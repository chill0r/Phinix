﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Authentication;
using Chat;
using Connections;
using HugsLib;
using HugsLib.Settings;
using RimWorld;
using Trading;
using HugsLib.Utils;
using UserManagement;
using Utils;
using Verse;
using Verse.Sound;
using Thing = Verse.Thing;

namespace PhinixClient
{
    public class Client : ModBase
    {
        public static Client Instance;
        public static readonly Version Version = Assembly.GetAssembly(typeof(Client)).GetName().Version;
        public void Log(LogEventArgs e) => ILoggableHandler(null, e);

        public override string ModIdentifier => "Phinix";

        private NetClient netClient;
        public bool Connected => netClient.Connected;
        public void Send(string module, byte[] serialisedMessage) => netClient.Send(module, serialisedMessage);
        public event EventHandler OnConnecting;
        public event EventHandler OnDisconnect;

        private ClientAuthenticator authenticator;
        public bool Authenticated => authenticator.Authenticated;
        public string SessionId => authenticator.SessionId;
        public event EventHandler<AuthenticationEventArgs> OnAuthenticationSuccess;
        public event EventHandler<AuthenticationEventArgs> OnAuthenticationFailure;

        private ClientUserManager userManager;
        public bool LoggedIn => userManager.LoggedIn;
        public string Uuid => userManager.Uuid;
        public bool TryGetDisplayName(string uuid, out string displayName) => userManager.TryGetDisplayName(uuid, out displayName);
        public bool TryGetUser(string uuid, out ImmutableUser user) => userManager.TryGetUser(uuid, out user);
        public string[] GetUserUuids(bool loggedIn = false) => userManager.GetUuids(loggedIn);
        public ImmutableUser[] GetUsers(bool loggedIn = false) => userManager.GetUsers(loggedIn);
        public event EventHandler<LoginEventArgs> OnLoginSuccess;
        public event EventHandler<LoginEventArgs> OnLoginFailure;
        public event EventHandler<UserDisplayNameChangedEventArgs> OnUserDisplayNameChanged;
        public event EventHandler<UserLoginStateChangedEventArgs> OnUserLoggedIn;
        public event EventHandler<UserLoginStateChangedEventArgs> OnUserLoggedOut;
        public event EventHandler<UserCreatedEventArgs> OnUserCreated;
        public event EventHandler OnUserSync;

        public bool Online => Connected && Authenticated && LoggedIn;

        private ClientChat chat;
        public void SendMessage(string message) => chat.Send(message);
        public void MarkAsRead() => chat.MarkAsRead();
        public UIChatMessage[] GetUnreadChatMessages(bool markAsRead = true) => GetChatMessages(markAsRead, true);
        public int UnreadMessages => chat.UnreadMessages;
        public int UnreadMessagesExcludingBlocked => chat.GetUnreadMessagesExcluding(BlockedUsers);
        public event EventHandler<UIChatMessageEventArgs> OnChatMessageReceived;
        public event EventHandler OnChatSync;

        private ClientTrading trading;
        public void CreateTrade(string uuid) => trading.CreateTrade(uuid);
        public void CancelTrade(string tradeId) => trading.CancelTrade(tradeId);
        public string[] GetTrades() => trading.GetTrades();
        public string[] GetTradesExceptWith(IEnumerable<string> otherPartyUuids) => trading.GetTradesExceptWith(otherPartyUuids);
        public bool TryGetOtherPartyUuid(string tradeId, out string otherPartyUuid) => trading.TryGetOtherPartyUuid(tradeId, out otherPartyUuid);
        public bool TryGetOtherPartyAccepted(string tradeId, out bool otherPartyAccepted) => trading.TryGetOtherPartyAccepted(tradeId, out otherPartyAccepted);
        public bool TryGetPartyAccepted(string tradeId, string partyUuid, out bool accepted) => trading.TryGetPartyAccepted(tradeId, partyUuid, out accepted);
        public bool TryGetItemsOnOffer(string tradeId, string uuid, out IEnumerable<Trading.ProtoThing> items) => trading.TryGetItemsOnOffer(tradeId, uuid, out items);
        public void UpdateTradeItems(string tradeId, IEnumerable<ProtoThing> items, string token = "") => trading.UpdateItems(tradeId, items, token);
        public void UpdateTradeStatus(string tradeId, bool? accepted = null, bool? cancelled = null) => trading.UpdateStatus(tradeId, accepted, cancelled);
        public LookTargets DropPods(IEnumerable<Thing> verseThings) => dropPods(verseThings);
        public event EventHandler<CreateTradeEventArgs> OnTradeCreationSuccess;
        public event EventHandler<CreateTradeEventArgs> OnTradeCreationFailure;
        public event EventHandler<CompleteTradeEventArgs> OnTradeCompleted;
        public event EventHandler<CompleteTradeEventArgs> OnTradeCancelled;
        public event EventHandler<TradeUpdateEventArgs> OnTradeUpdateSuccess;
        public event EventHandler<TradeUpdateEventArgs> OnTradeUpdateFailure;
        public event EventHandler<TradesSyncedEventArgs> OnTradesSynced;

        public event EventHandler<BlockedUsersChangedEventArgs> OnBlockedUsersChanged;

        private SettingHandle<string> serverAddressHandle;
        public string ServerAddress
        {
            get => serverAddressHandle.Value;
            set
            {
                serverAddressHandle.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<int> serverPortHandle;
        public int ServerPort
        {
            get => serverPortHandle.Value;
            set
            {
                serverPortHandle.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<string> displayNameHandle;
        public string DisplayName
        {
            get => displayNameHandle.Value;
            set
            {
                displayNameHandle.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<bool> acceptingTradesHandle;
        public bool AcceptingTrades
        {
            get => acceptingTradesHandle.Value;
            set
            {
                acceptingTradesHandle.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<bool> showNameFormatting;
        public bool ShowNameFormatting
        {
            get => showNameFormatting.Value;
            set
            {
                showNameFormatting.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<bool> showChatFormatting;
        public bool ShowChatFormatting
        {
            get => showChatFormatting.Value;
            set
            {
                showChatFormatting.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<bool> playNoiseOnMessageReceived;
        public bool PlayNoiseOnMessageReceived
        {
            get => playNoiseOnMessageReceived.Value;
            set
            {
                playNoiseOnMessageReceived.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<bool> showUnreadMessageCount;
        public bool ShowUnreadMessageCount
        {
            get => showUnreadMessageCount.Value;
            set
            {
                showUnreadMessageCount.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<bool> showBlockedUnreadMessageCount;
        public bool ShowBlockedUnreadMessageCount
        {
            get => showBlockedUnreadMessageCount.Value;
            set
            {
                showBlockedUnreadMessageCount.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<bool> allItemsTradable;
        public bool AllItemsTradable
        {
            get => allItemsTradable.Value;
            set
            {
                allItemsTradable.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<bool> showBlockedTrades;
        public bool ShowBlockedTrades
        {
            get => showBlockedTrades.Value;
            set
            {
                showBlockedTrades.Value = value;
                HugsLibController.SettingsManager.SaveChanges();
            }
        }

        private SettingHandle<ListSetting<string>> blockedUsers;
        public List<string> BlockedUsers => blockedUsers.Value.List;

        /// <summary>
        /// Queue of sounds to play on the next frame.
        /// Necessary because sounds are only played on the main Unity thread.
        /// </summary>
        private Queue<SoundDef> soundQueue = new Queue<SoundDef>();
        /// <summary>
        /// Lock object to prevent race conditions when accessing soundQueue.
        /// </summary>
        private object soundQueueLock = new object();

        /// <inheritdoc />
        /// <summary>
        /// Called by HugsLib shortly after the mod is loaded.
        /// Used for initial setup only.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Client.Instance = this;

            // Load in Settings
            serverAddressHandle = Settings.GetHandle(
                settingName: "serverAddress",
                title: "Phinix_hugslibsettings_serverAddressTitle".Translate(),
                description: null,
                defaultValue: "phinix.chat"
            );
            serverPortHandle = Settings.GetHandle(
                settingName: "serverPort",
                title: "Phinix_hugslibsettings_serverPortTitle".Translate(),
                description: null,
                defaultValue: 16200,
                validator: value => int.TryParse(value, out _)
            );
            displayNameHandle = Settings.GetHandle(
                settingName: "displayName",
                title: "Phinix_hugslibsettings_displayNameTitle".Translate(),
                description: null,
                defaultValue: SteamUtility.SteamPersonaName
            );
            acceptingTradesHandle = Settings.GetHandle(
                settingName: "acceptingTrades",
                title: "Phinix_hugslibsettings_acceptingTradesTitle".Translate(),
                description: null,
                defaultValue: true
            );
            showNameFormatting = Settings.GetHandle(
                settingName: "showNameFormatting",
                title: "Phinix_hugslibsettings_showNameFormatting".Translate(),
                description: null,
                defaultValue: true
            );
            showChatFormatting = Settings.GetHandle(
                settingName: "showChatFormatting",
                title: "Phinix_hugslibsettings_showChatFormatting".Translate(),
                description: null,
                defaultValue: true
            );
            playNoiseOnMessageReceived = Settings.GetHandle(
                settingName: "playNoiseOnMessageReceived",
                title: "Phinix_hugslibsettings_playNoiseOnMessageReceived".Translate(),
                description: null,
                defaultValue: true
            );
            showUnreadMessageCount = Settings.GetHandle(
                settingName: "showUnreadMessageCount",
                title: "Phinix_hugslibsettings_showUnreadMessageCount".Translate(),
                description: null,
                defaultValue: true
            );
            showBlockedUnreadMessageCount = Settings.GetHandle(
                settingName: "showUnreadMessageCount",
                title: "Phinix_hugslibsettings_showBlockedUnreadMessageCount".Translate(),
                description: "Phinix_hugslibsettings_showBlockedUnreadMessageCount_description".Translate(),
                defaultValue: true
            );
            allItemsTradable = Settings.GetHandle(
                settingName: "allItemsTradable",
                title: "Phinix_hugslibsettings_allItemsTradable".Translate(),
                description: null,
                defaultValue: false
            );
            showBlockedTrades = Settings.GetHandle(
                settingName: "showBlockedTrades",
                title: "Phinix_hugslibsettings_showBlockedTrades".Translate(),
                description: null,
                defaultValue: false
            );
            blockedUsers = Settings.GetHandle<ListSetting<string>>(
                settingName: "blockedUsers",
                title: "Phinix_hugslibsettings_blockedUsers".Translate(),
                description: null
            );
            blockedUsers.NeverVisible = true;
            // Always initialise a new value otherwise it will use the reference of the default value, resulting in the
            // default list being updated and the save mechanism never being able to differentiate any changes.
            if (blockedUsers.Value == null) blockedUsers.Value = new ListSetting<string>();

            // Set up our module instances
            this.netClient = new NetClient();
            this.authenticator = new ClientAuthenticator(netClient, getCredentials);
            this.userManager = new ClientUserManager(netClient, authenticator);
            this.chat = new ClientChat(netClient, authenticator, userManager);
            this.trading = new ClientTrading(netClient, authenticator, userManager);

            // Subscribe to log events
            authenticator.OnLogEntry += ILoggableHandler;
            userManager.OnLogEntry += ILoggableHandler;
            chat.OnLogEntry += ILoggableHandler;
            trading.OnLogEntry += ILoggableHandler;

            // Subscribe to authentication events
            authenticator.OnAuthenticationSuccess += (sender, args) =>
            {
                Logger.Message("Successfully authenticated with server.");
                userManager.SendLogin(
                    displayName: DisplayName,
                    acceptingTrades: AcceptingTrades
                );
            };
            authenticator.OnAuthenticationFailure += (sender, args) =>
            {
                Logger.Message("Failed to authenticate with server: {0} ({1})", args.FailureMessage, args.FailureReason.ToString());

                Find.WindowStack.Add(new Dialog_Message("Phinix_error_authFailedTitle".Translate(), "Phinix_error_authFailedMessage".Translate(args.FailureMessage, args.FailureReason.ToString())));

                Disconnect();
            };

            // Subscribe to user management events
            userManager.OnLoginSuccess += (sender, args) =>
            {
                Logger.Message("Successfully logged in with UUID {0}", userManager.Uuid);
            };
            userManager.OnLoginFailure += (sender, args) =>
            {
                Logger.Message("Failed to log in to server: {0} ({1})", args.FailureMessage, args.FailureReason.ToString());

                Find.WindowStack.Add(new Dialog_Message("Phinix_error_loginFailedTitle".Translate(), "Phinix_error_loginFailedMessage".Translate(args.FailureMessage, args.FailureReason.ToString())));

                Disconnect();
            };
            userManager.OnUserDisplayNameChanged += (sender, args) =>
            {
                Logger.Trace(string.Format("User with UUID {0} changed their display name from \"{1}\" to \"{2}\"", args.Uuid, args.OldDisplayName, args.NewDisplayName));
            };
            userManager.OnUserLoggedIn += (sender, args) =>
            {
                Logger.Trace(string.Format("User {0} logged in", args.Uuid));
            };
            userManager.OnUserLoggedOut += (sender, args) =>
            {
                Logger.Trace(string.Format("User {0} logged out", args.Uuid));
            };
            userManager.OnUserCreated += (sender, args) =>
            {
                Logger.Trace(string.Format("New user created: {0} ({1}) - {2}gged in", args.DisplayName, args.Uuid, args.LoggedIn ? "L" : "Not l"));
            };

            // Subscribe to chat events
            chat.OnChatMessageReceived += (sender, args) =>
            {
                Logger.Trace("Received chat message from UUID " + args.Message.SenderUuid);

                // Check if the message wasn't ours, chat noises are enabled, and if we are in-game before playing a sound
                if (args.Message.SenderUuid != Uuid && PlayNoiseOnMessageReceived && Current.Game != null && !BlockedUsers.Contains(args.Message.SenderUuid))
                {
                    lock (soundQueueLock)
                    {
                        // Add a little tick noise to the sound queue
                        // (queue is necessary because sounds only play on the main Unity thread)
                        soundQueue.Enqueue(SoundDefOf.Tick_Tiny);
                    }
                }
            };

            // Subscribe to trading events
            trading.OnTradeCreationSuccess += (sender, args) =>
            {
                Logger.Trace(string.Format("Created trade {0} with {1}", args.TradeId, args.OtherPartyUuid));

                // Don't display anything if the other party is blocked and we want to hide their trades
                if (!ShowBlockedTrades && Instance.BlockedUsers.Contains(args.OtherPartyUuid)) return;

                // Try get the other party's display name
                if (Instance.TryGetDisplayName(args.OtherPartyUuid, out string displayName))
                {
                    // Strip formatting
                    displayName = TextHelper.StripRichText(displayName);
                }
                else
                {
                    // Unknown display name, default to ???
                    displayName = "???";
                }

                // Generate a letter
                LetterDef letterDef = DefDatabase<LetterDef>.GetNamed("TradeCreated");
                Find.LetterStack.ReceiveLetter(
                    label: "Phinix_trade_tradeReceivedLetter_label".Translate(displayName),
                    text: "Phinix_trade_tradeReceivedLetter_description".Translate(displayName),
                    textLetterDef: letterDef
                );
            };
            trading.OnTradeCreationFailure += (sender, args) =>
            {
                Logger.Trace(string.Format("Failed to create trade with {0}: {1} ({2})", args.OtherPartyUuid, args.FailureMessage, args.FailureReason.ToString()));

                Find.WindowStack.Add(new Dialog_Message("Phinix_error_tradeCreationFailedTitle".Translate(), "Phinix_error_tradeCreationFailedMessage".Translate(args.FailureMessage, args.FailureReason.ToString())));
            };
            trading.OnTradeCompleted += (sender, args) =>
            {
                // Try get the other party's display name
                if (Instance.TryGetDisplayName(args.OtherPartyUuid, out string displayName))
                {
                    // Strip formatting
                    displayName = TextHelper.StripRichText(displayName);
                }
                else
                {
                    // Unknown display name, default to ???
                    displayName = "???";
                }

                // Convert all the received items into their Verse counterparts and strip out any unknown ones
                //// While it would be less computationally-expensive to strip out unknown items beforehand, we would
                //// have no idea whether we could actually make the item without another check, so we just piggy-back
                //// off of the converter's checks and strip them out afterward.
                Verse.Thing[] verseItems = args.Items
                                                .Select(TradingThingConverter.ConvertThingFromProtoOrUnknown)
                                                .Where(thing => thing.def.defName != "UnknownItem")
                                                .ToArray();


                // Launch drop pods to a trade spot on a home tile
                LookTargets dropSpotLookTarget = dropPods(verseItems);

                // Generate a letter
                LetterDef letterDef = DefDatabase<LetterDef>.GetNamed("TradeAccepted");
                Find.LetterStack.ReceiveLetter("Phinix_trade_tradeCompletedLetter_label".Translate(), "Phinix_trade_tradeCompletedLetter_description".Translate(displayName), letterDef, dropSpotLookTarget);

                Logger.Trace(string.Format("Trade with {0} completed successfully", args.OtherPartyUuid));
            };
            trading.OnTradeCancelled += (sender, args) =>
            {
                // Don't display anything if the other party is blocked and we want to hide their trades
                if (!ShowBlockedTrades && Instance.BlockedUsers.Contains(args.OtherPartyUuid)) return;

                // Try get the other party's display name
                if (userManager.TryGetDisplayName(args.OtherPartyUuid, out string displayName))
                {
                    // Strip formatting
                    displayName = TextHelper.StripRichText(displayName);
                }
                else
                {
                    // Unknown display name, default to ???
                    displayName = "???";
                }

                // Convert all the received items into their Verse counterparts and strip out any unknown ones
                //// While it would be less computationally-expensive to strip out unknown items beforehand, we would
                //// have no idea whether we could actually make the item without another check, so we just piggy-back
                //// off of the converter's checks and strip them out afterward.
                Verse.Thing[] verseItems = args.Items
                                                .Select(TradingThingConverter.ConvertThingFromProtoOrUnknown)
                                                .Where(thing => thing.def.defName != "UnknownItem")
                                                .ToArray();

                // Launch drop pods to a trade spot on a home tile
                LookTargets dropSpotLookTarget = dropPods(verseItems);

                // Generate a letter
                LetterDef letterDef = DefDatabase<LetterDef>.GetNamed("TradeCancelled");
                Find.LetterStack.ReceiveLetter("Phinix_trade_tradeCancelled_label".Translate(), "Phinix_trade_tradeCancelled_description".Translate(displayName), letterDef, dropSpotLookTarget);

                Logger.Trace(string.Format("Trade with {0} cancelled", args.OtherPartyUuid));
            };
            trading.OnTradeUpdateFailure += (sender, args) =>
            {
                // Try get the other party's display name
                if (trading.TryGetOtherPartyUuid(args.TradeId, out string otherPartyUuid) &&
                    userManager.TryGetDisplayName(otherPartyUuid, out string displayName))
                {
                    // Strip formatting
                    displayName = TextHelper.StripRichText(displayName);
                }
                else
                {
                    // Unknown display name, default to ???
                    displayName = "???";
                }

                Find.WindowStack.Add(new Dialog_Message("Phinix_error_tradeUpdateFailedTitle".Translate(), "Phinix_error_tradeUpdateFailedMessage".Translate(displayName, args.FailureMessage, args.FailureReason.ToString())));
            };
            trading.OnTradesSynced += (sender, args) =>
            {
                Logger.Trace(string.Format("Synced {0} trade{1} from server", args.TradeIds.Length, args.TradeIds.Length != 1 ? "s" : ""));
            };

            // Subscribe to setting handle value change events
            acceptingTradesHandle.ValueChanged += (handle) => { userManager.UpdateSelf(acceptingTrades: acceptingTradesHandle.Value); };

            // Forward events so the UI can handle them
            netClient.OnConnecting += (sender, e) => { OnConnecting?.Invoke(sender, e); };
            netClient.OnDisconnect += (sender, e) => { OnDisconnect?.Invoke(sender, e); };
            authenticator.OnAuthenticationSuccess += (sender, e) => { OnAuthenticationSuccess?.Invoke(sender, e); };
            authenticator.OnAuthenticationFailure += (sender, e) => { OnAuthenticationFailure?.Invoke(sender, e); };
            userManager.OnLoginSuccess += (sender, e) => { OnLoginSuccess?.Invoke(sender, e); };
            userManager.OnLoginFailure += (sender, e) => { OnLoginFailure?.Invoke(sender, e); };
            userManager.OnUserDisplayNameChanged += (sender, e) => { OnUserDisplayNameChanged?.Invoke(sender, e); };
            userManager.OnUserLoggedIn += (sender, e) => { OnUserLoggedIn?.Invoke(sender, e); };
            userManager.OnUserLoggedOut += (sender, e) => { OnUserLoggedOut?.Invoke(sender, e); };
            userManager.OnUserCreated += (sender, e) => { OnUserCreated?.Invoke(sender, e); };
            userManager.OnUserSync += (sender, e) => { OnUserSync?.Invoke(sender, e); };
            chat.OnChatMessageReceived += (sender, e) => { OnChatMessageReceived?.Invoke(sender, new UIChatMessageEventArgs(new UIChatMessage(userManager, e.Message))); };
            chat.OnChatSync += (sender, e) => { OnChatSync?.Invoke(sender, e); };
            trading.OnTradeCreationSuccess += (sender, e) => { OnTradeCreationSuccess?.Invoke(sender, e); };
            trading.OnTradeCreationFailure += (sender, e) => { OnTradeCreationFailure?.Invoke(sender, e); };
            trading.OnTradeCompleted += (sender, e) => { OnTradeCompleted?.Invoke(sender, e); };
            trading.OnTradeCancelled += (sender, e) => { OnTradeCancelled?.Invoke(sender, e); };
            trading.OnTradeUpdateSuccess += (sender, e) => { OnTradeUpdateSuccess?.Invoke(sender, e); };
            trading.OnTradeUpdateFailure += (sender, e) => { OnTradeUpdateFailure?.Invoke(sender, e); };
            trading.OnTradesSynced += (sender, e) => { OnTradesSynced?.Invoke(sender, e); };

            // Connect to the server set in the config
            Connect(ServerAddress, ServerPort);
        }

        /// <summary>
        /// Adds a user's UUID to the blocked user list.
        /// </summary>
        /// <param name="senderUuid">UUID of user to block</param>
        public void BlockUser(string senderUuid)
        {
            BlockedUsers.AddDistinct(senderUuid);
            blockedUsers.HasUnsavedChanges = true;
            HugsLibController.SettingsManager.SaveChanges();

            OnBlockedUsersChanged?.Invoke(this, new BlockedUsersChangedEventArgs(senderUuid, true));
        }

        /// <summary>
        /// Removes a user's UUID from the blocked user list.
        /// </summary>
        /// <param name="senderUuid">UUID of the user to unblock</param>
        public void UnBlockUser(string senderUuid)
        {
            BlockedUsers.Remove(senderUuid);
            blockedUsers.HasUnsavedChanges = true;
            HugsLibController.SettingsManager.SaveChanges();

            OnBlockedUsersChanged?.Invoke(this, new BlockedUsersChangedEventArgs(senderUuid, false));
        }

        /// <inheritdoc />
        public override void Update()
        {
            lock (soundQueueLock)
            {
                // Check if we have sounds to play
                while (soundQueue.Count > 0)
                {
                    // Dequeue and play a sound
                    soundQueue.Dequeue().PlayOneShotOnCamera();
                }
            }
        }

        /// <summary>
        /// Attempts to connect to the server at the given address and port.
        /// This will disconnect from the current server, if any.
        /// </summary>
        /// <param name="address">Server address</param>
        /// <param name="port">Server port</param>
        public void Connect(string address, int port)
        {
            if (Connected) Disconnect();

            try
            {
                netClient.Connect(address, port);
            }
            catch
            {
                Logger.Message("Could not connect to {0}:{1}", ServerAddress, ServerPort);

                Find.WindowStack.Add(new Dialog_Message("Phinix_error_connectionFailedTitle".Translate(), "Phinix_error_connectionFailedMessage".Translate(ServerAddress, ServerPort)));
            }
        }

        /// <summary>
        /// If connected, disconnects from the current server.
        /// </summary>
        public void Disconnect()
        {
            netClient.Disconnect();
        }

        /// <summary>
        /// Updates the user's display name locally and on the server.
        /// </summary>
        /// <param name="displayName">Display name</param>
        public void UpdateDisplayName(string displayName)
        {
            // Try to update within the user manager
            userManager.UpdateSelf(displayName);
        }

        /// <summary>
        /// Gets the current chat message buffer, optionally marking them as read.
        /// </summary>
        /// <param name="markAsRead">Whether to mark the messages as read</param>
        /// <param name="unreadOnly">Whether to only get unread messages</param>
        /// <returns>List of chat messages</returns>
        public UIChatMessage[] GetChatMessages(bool markAsRead = true, bool unreadOnly = false)
        {
            ClientChatMessage[] chatMessages = unreadOnly ? chat.GetUnreadMessages(markAsRead) : chat.GetMessages(markAsRead);

            return chatMessages.Select(m => new UIChatMessage(userManager, m)).ToArray();
        }

        /// <summary>
        /// Tries to get the chat message with the given ID.
        /// </summary>
        /// <param name="messageId">ID of the chat message to retrieve</param>
        /// <param name="message">Chat message output</param>
        /// <returns>Whether the chat message was retrieved successfully</returns>
        public bool TryGetMessage(string messageId, out UIChatMessage message)
        {
            message = null;

            // Try pull out the message
            if (!chat.TryGetMessage(messageId, out ClientChatMessage clientChatMessage)) return false;

            // Wrap it with the sender's user details
            message = new UIChatMessage(userManager, clientChatMessage);

            return true;
        }

        /// <summary>
        /// Handler for <see cref="ILoggable"/> <c>OnLogEvent</c> events.
        /// Raised by modules as a way to hook into the HugsLib log.
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="args">Event arguments</param>
        private void ILoggableHandler(object sender, LogEventArgs args)
        {
            switch (args.LogLevel)
            {
                case LogLevel.DEBUG:
                    Logger.Trace(args.Message);
                    break;
                case LogLevel.WARNING:
                    Logger.Warning(args.Message);
                    break;
                case LogLevel.ERROR:
                case LogLevel.FATAL:
                    Logger.Error(args.Message);
                    break;
                case LogLevel.INFO:
                default:
                    Logger.Message(args.Message);
                    break;
            }
        }

        /// <summary>
        /// Handles credential requests from the <see cref="ClientAuthenticator"/> module.
        /// This forwards the server details and a callback to the GUI for user input.
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <param name="serverName">Server name</param>
        /// <param name="serverDescription">Server description</param>
        /// <param name="authType">Authentication type</param>
        /// <param name="callback">Callback delegate to pass entered credentials to</param>
        private void getCredentials(string sessionId, string serverName, string serverDescription, AuthTypes authType, ClientAuthenticator.ReturnCredentialsDelegate callback)
        {
            Logger.Trace("Authentication needs more credentials for the server \"{0}\" with authentication type \"{1}\"", serverName, authType.ToString());

            Find.WindowStack.Add(new CredentialsWindow
            {
                SessionId = sessionId,
                ServerName = serverName,
                ServerDescription = serverDescription,
                AuthType = authType,
                CredentialsCallback = callback
            });
        }

        /// <summary>
        /// Launches the given <see cref="Thing"/>s in drop pods to a trade spot at the home colony.
        /// </summary>
        /// <param name="things">Collection of <see cref="Thing"/>s to drop</param>
        /// <returns>LookTarget for the drop location</returns>
        private LookTargets dropPods(IEnumerable<Thing> things)
        {
            // Launch drop pods to a trade spot on a home tile
            Map map = Find.AnyPlayerHomeMap;
            IntVec3 dropSpot = DropCellFinder.TradeDropSpot(map);
            DropPodUtility.DropThingsNear(dropSpot, map, things, canRoofPunch: false);

            return new LookTargets(dropSpot, map);
        }
    }
}
