﻿using System;
using System.Linq;
using System.Reflection;
using Utils;

namespace UserManagement
{
    /// <inheritdoc />
    /// <summary>
    /// Module that organises users, their credentials, and their login states.
    /// Anything and everything pertaining to a particular user's state is accessed through this.
    /// </summary>
    public abstract class UserManager : ILoggable
    {
        public readonly string MODULE_NAME = typeof(UserManager).Namespace;
        
        public static readonly Version Version = Assembly.GetAssembly(typeof(UserManager)).GetName().Version;
        
        /// <inheritdoc />
        public abstract event EventHandler<LogEventArgs> OnLogEntry;
        /// <inheritdoc />
        public abstract void RaiseLogEntry(LogEventArgs e);

        /// <summary>
        /// Stores each user in an easily-serialisable format.
        /// </summary>
        protected abstract UserStore userStore { get; set; }

        /// <summary>
        /// Lock for user store operations.
        /// </summary>
        protected abstract object userStoreLock { get; }

        /// <summary>
        /// Tries to get a user's UUID by their username and returns whether it was retrieved successfully.
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="uuid">UUID of the user</param>
        /// <returns>Successfully retrieved user</returns>
        /// <exception cref="ArgumentException">UUID cannot be null or empty</exception>
        public bool TryGetUserUuid(string username, out string uuid)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentException("Username cannot be null or empty.", nameof(uuid));

            // Initialise UUID to something arbitrary
            uuid = null;

            lock (userStoreLock)
            {
                User matchingUser;
                try
                {
                    matchingUser = userStore.Users.Values.Single(user => user.Username == username);
                }
                catch (InvalidOperationException)
                {
                    return false;
                }
                
                uuid = matchingUser.Uuid;
            }
            
            return true;
        }
        
        /// <summary>
        /// Creates a new user with the given display name, optionally logged-in, and adds them to the user list.
        /// Returns the UUID of the newly-created user.
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="displayName">Display name of the user</param>
        /// <param name="loggedIn">Whether the user should be logged in</param>
        /// <returns>UUID of created user</returns>
        /// <exception cref="ArgumentException">Display name cannot be null or empty</exception>
        public string CreateUser(string username, string displayName, bool loggedIn = false)
        {
            if (string.IsNullOrEmpty(displayName)) throw new ArgumentException("Display name cannot be null or empty.", nameof(displayName));

            User user = new User
            {
                Uuid = Guid.NewGuid().ToString(),
                Username = username,
                DisplayName = string.IsNullOrEmpty(displayName) ? "" : displayName,
                LoggedIn = loggedIn
            };

            lock (userStoreLock)
            {
                userStore.Users.Add(user.Uuid, user);
            }

            return user.Uuid;
        }

        /// <summary>
        /// Updates an existing user's properties.
        /// Returns true if the update was successful, otherwise false.
        /// </summary>
        /// <param name="uuid">UUID of the user</param>
        /// <param name="displayName">Display name of the user</param>
        /// <param name="acceptingTrades">Whether the user is accepting trades</param>
        /// <returns>User updated successfully</returns>
        /// <exception cref="ArgumentNullException">UUID cannot be null or empty</exception>
        public virtual bool UpdateUser(string uuid, string displayName = null, bool? acceptingTrades = null)
        {
            if (string.IsNullOrEmpty(uuid)) throw new ArgumentException("UUID cannot be null or empty.", nameof(uuid));

            lock (userStoreLock)
            {
                if (!userStore.Users.ContainsKey(uuid)) return false;

                if (displayName != null) userStore.Users[uuid].DisplayName = displayName;

                if (acceptingTrades.HasValue) userStore.Users[uuid].AcceptingTrades = acceptingTrades.Value;
            }

            return true;
        }

        /// <summary>
        /// Removes an existing user by their UUID.
        /// Returns true if the user was removed successfully, otherwise false.
        /// </summary>
        /// <param name="uuid">UUID of the user</param>
        /// <returns>User removed successfully</returns>
        /// <exception cref="ArgumentException">UUID cannot be null or empty</exception>
        public bool RemoveUser(string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) throw new ArgumentException("UUID cannot be null or empty.", nameof(uuid));

            lock (userStoreLock)
            {
                return userStore.Users.Remove(uuid);
            }
        }

        /// <summary>
        /// Attempts to log in a user with the given UUID.
        /// Returns whether the user was successfully logged in.
        /// </summary>
        /// <param name="uuid">UUID of the user</param>
        /// <returns>Whether the user was successfully logged in</returns>
        /// <exception cref="ArgumentException">UUID cannot be null or empty</exception>
        public virtual bool TryLogIn(string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) throw new ArgumentException("UUID cannot be null or empty.", nameof(uuid));

            lock (userStoreLock)
            {
                if (!userStore.Users.ContainsKey(uuid)) return false;

                userStore.Users[uuid].LoggedIn = true;
            }

            // TODO: Broadcast the state change
            return true;
        }
        
        /// <summary>
        /// Attempts to log out a user with the given UUID.
        /// Returns whether the user was successfully logged out.
        /// </summary>
        /// <param name="uuid">UUID of the user</param>
        /// <returns>Whether the user was successfully logged out</returns>
        /// <exception cref="ArgumentException">UUID cannot be null or empty</exception>
        public virtual bool TryLogOut(string uuid)
        {
            if (string.IsNullOrEmpty(uuid)) throw new ArgumentException("UUID cannot be null or empty.", nameof(uuid));

            lock (userStoreLock)
            {
                if (!userStore.Users.ContainsKey(uuid)) return false;

                userStore.Users[uuid].LoggedIn = false;
            }

            return true;
        }

        /// <summary>
        /// Attempts to get the logged-in state of a user with the given UUID.
        /// Returns whether the logged-in state was retrieved successfully.
        /// </summary>
        /// <param name="uuid">UUID of the user</param>
        /// <param name="loggedIn">Output logged-in state</param>
        /// <returns>Logged-in state was retrieved successfully</returns>
        /// <exception cref="ArgumentException">UUID cannot be null or empty</exception>
        public bool TryGetLoggedIn(string uuid, out bool loggedIn)
        {
            // Initialise logged in to something arbitrary
            loggedIn = false;
            
            if (string.IsNullOrEmpty(uuid)) throw new ArgumentException("UUID cannot be null or empty.", nameof(uuid));

            lock (userStoreLock)
            {
                if (!userStore.Users.ContainsKey(uuid)) return false;
                
                loggedIn = userStore.Users[uuid].LoggedIn;
            }
            
            return true;
        }

        /// <summary>
        /// Attempts to get the display name of a user with the given UUID.
        /// Returns whether the display name was retrieved successfully.
        /// </summary>
        /// <param name="uuid">UUID of the user</param>
        /// <param name="displayName">Output display name</param>
        /// <returns>Display name was retrieved successfully</returns>
        /// <exception cref="ArgumentException">UUID cannot be null or empty</exception>
        public bool TryGetDisplayName(string uuid, out string displayName)
        {
            // Initialise display name to something arbitrary
            displayName = null;
            
            if (string.IsNullOrEmpty(uuid)) throw new ArgumentException("UUID cannot be null or empty.", nameof(uuid));

            lock (userStoreLock)
            {
                if (!userStore.Users.ContainsKey(uuid)) return false;

                displayName = userStore.Users[uuid].DisplayName;
            }

            return true;
        }

        /// <summary>
        /// Attempts to get the accepting trades state of a user with the given UUID.
        /// Returns whether the accepting trades state was retrieved successfully.
        /// </summary>
        /// <param name="uuid">UUID of the user</param>
        /// <param name="acceptingTrades">Output accepting trades state</param>
        /// <returns>accepting trades state was retrieved successfully</returns>
        /// <exception cref="ArgumentException">UUID cannot be null or empty</exception>
        public bool TryGetAcceptingTrades(string uuid, out bool acceptingTrades)
        {
            // Initialise accepting trades to something arbitrary
            acceptingTrades = false;
            
            if (string.IsNullOrEmpty(uuid)) throw new ArgumentException("UUID cannot be null or empty.", nameof(uuid));

            lock (userStoreLock)
            {
                if (!userStore.Users.ContainsKey(uuid)) return false;

                acceptingTrades = userStore.Users[uuid].AcceptingTrades;
            }

            return true;
        }

        /// <summary>
        /// Attempts to get the state of a user with the given UUID.
        /// Returns whether the user was retrieved successfully.
        /// </summary>
        /// <param name="uuid">UUID of the user</param>
        /// <param name="user">Output user state</param>
        /// <returns>User was retrieved successfully</returns>
        /// <exception cref="ArgumentException">UUID cannot be null or empty</exception>
        public bool TryGetUser(string uuid, out ImmutableUser user)
        {
            // Initialise user to something arbitrary
            user = new ImmutableUser();

            if (string.IsNullOrEmpty(uuid)) throw new ArgumentException("UUID cannot be null or empty.", nameof(uuid));

            lock (userStoreLock)
            {
                if (!userStore.Users.ContainsKey(uuid)) return false;

                user = new ImmutableUser(uuid, userStore.Users[uuid].DisplayName, userStore.Users[uuid].LoggedIn, userStore.Users[uuid].AcceptingTrades);
            }

            return true;
        }
    }
}
