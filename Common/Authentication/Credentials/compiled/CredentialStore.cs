// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: Credentials/CredentialStore.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Authentication {

  /// <summary>Holder for reflection information generated from Credentials/CredentialStore.proto</summary>
  public static partial class CredentialStoreReflection {

    #region Descriptor
    /// <summary>File descriptor for Credentials/CredentialStore.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CredentialStoreReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiFDcmVkZW50aWFscy9DcmVkZW50aWFsU3RvcmUucHJvdG8SDkF1dGhlbnRp",
            "Y2F0aW9uGhxDcmVkZW50aWFscy9DcmVkZW50aWFsLnByb3RvIqgBCg9DcmVk",
            "ZW50aWFsU3RvcmUSRQoLQ3JlZGVudGlhbHMYASADKAsyMC5BdXRoZW50aWNh",
            "dGlvbi5DcmVkZW50aWFsU3RvcmUuQ3JlZGVudGlhbHNFbnRyeRpOChBDcmVk",
            "ZW50aWFsc0VudHJ5EgsKA2tleRgBIAEoCRIpCgV2YWx1ZRgCIAEoCzIaLkF1",
            "dGhlbnRpY2F0aW9uLkNyZWRlbnRpYWw6AjgBYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Authentication.CredentialReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Authentication.CredentialStore), global::Authentication.CredentialStore.Parser, new[]{ "Credentials" }, null, null, new pbr::GeneratedClrTypeInfo[] { null, })
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class CredentialStore : pb::IMessage<CredentialStore> {
    private static readonly pb::MessageParser<CredentialStore> _parser = new pb::MessageParser<CredentialStore>(() => new CredentialStore());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CredentialStore> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Authentication.CredentialStoreReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CredentialStore() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CredentialStore(CredentialStore other) : this() {
      credentials_ = other.credentials_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CredentialStore Clone() {
      return new CredentialStore(this);
    }

    /// <summary>Field number for the "Credentials" field.</summary>
    public const int CredentialsFieldNumber = 1;
    private static readonly pbc::MapField<string, global::Authentication.Credential>.Codec _map_credentials_codec
        = new pbc::MapField<string, global::Authentication.Credential>.Codec(pb::FieldCodec.ForString(10), pb::FieldCodec.ForMessage(18, global::Authentication.Credential.Parser), 10);
    private readonly pbc::MapField<string, global::Authentication.Credential> credentials_ = new pbc::MapField<string, global::Authentication.Credential>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::MapField<string, global::Authentication.Credential> Credentials {
      get { return credentials_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CredentialStore);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CredentialStore other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!Credentials.Equals(other.Credentials)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= Credentials.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      credentials_.WriteTo(output, _map_credentials_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += credentials_.CalculateSize(_map_credentials_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CredentialStore other) {
      if (other == null) {
        return;
      }
      credentials_.Add(other.credentials_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            credentials_.AddEntriesFrom(input, _map_credentials_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code