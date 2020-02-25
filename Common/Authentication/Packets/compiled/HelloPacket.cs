// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Packets/HelloPacket.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Authentication {

  /// <summary>Holder for reflection information generated from Packets/HelloPacket.proto</summary>
  public static partial class HelloPacketReflection {

    #region Descriptor
    /// <summary>File descriptor for Packets/HelloPacket.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static HelloPacketReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChlQYWNrZXRzL0hlbGxvUGFja2V0LnByb3RvEg5BdXRoZW50aWNhdGlvbhoX",
            "UGFja2V0cy9BdXRoVHlwZXMucHJvdG8ifAoLSGVsbG9QYWNrZXQSEgoKU2Vy",
            "dmVyTmFtZRgBIAEoCRIZChFTZXJ2ZXJEZXNjcmlwdGlvbhgCIAEoCRIrCghB",
            "dXRoVHlwZRgDIAEoDjIZLkF1dGhlbnRpY2F0aW9uLkF1dGhUeXBlcxIRCglT",
            "ZXNzaW9uSWQYBCABKAliBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Authentication.AuthTypesReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Authentication.HelloPacket), global::Authentication.HelloPacket.Parser, new[]{ "ServerName", "ServerDescription", "AuthType", "SessionId" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class HelloPacket : pb::IMessage<HelloPacket> {
    private static readonly pb::MessageParser<HelloPacket> _parser = new pb::MessageParser<HelloPacket>(() => new HelloPacket());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<HelloPacket> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Authentication.HelloPacketReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloPacket() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloPacket(HelloPacket other) : this() {
      serverName_ = other.serverName_;
      serverDescription_ = other.serverDescription_;
      authType_ = other.authType_;
      sessionId_ = other.sessionId_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public HelloPacket Clone() {
      return new HelloPacket(this);
    }

    /// <summary>Field number for the "ServerName" field.</summary>
    public const int ServerNameFieldNumber = 1;
    private string serverName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ServerName {
      get { return serverName_; }
      set {
        serverName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "ServerDescription" field.</summary>
    public const int ServerDescriptionFieldNumber = 2;
    private string serverDescription_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ServerDescription {
      get { return serverDescription_; }
      set {
        serverDescription_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "AuthType" field.</summary>
    public const int AuthTypeFieldNumber = 3;
    private global::Authentication.AuthTypes authType_ = global::Authentication.AuthTypes.ClientKey;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Authentication.AuthTypes AuthType {
      get { return authType_; }
      set {
        authType_ = value;
      }
    }

    /// <summary>Field number for the "SessionId" field.</summary>
    public const int SessionIdFieldNumber = 4;
    private string sessionId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string SessionId {
      get { return sessionId_; }
      set {
        sessionId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as HelloPacket);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(HelloPacket other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (ServerName != other.ServerName) return false;
      if (ServerDescription != other.ServerDescription) return false;
      if (AuthType != other.AuthType) return false;
      if (SessionId != other.SessionId) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (ServerName.Length != 0) hash ^= ServerName.GetHashCode();
      if (ServerDescription.Length != 0) hash ^= ServerDescription.GetHashCode();
      if (AuthType != global::Authentication.AuthTypes.ClientKey) hash ^= AuthType.GetHashCode();
      if (SessionId.Length != 0) hash ^= SessionId.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (ServerName.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(ServerName);
      }
      if (ServerDescription.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(ServerDescription);
      }
      if (AuthType != global::Authentication.AuthTypes.ClientKey) {
        output.WriteRawTag(24);
        output.WriteEnum((int) AuthType);
      }
      if (SessionId.Length != 0) {
        output.WriteRawTag(34);
        output.WriteString(SessionId);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (ServerName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ServerName);
      }
      if (ServerDescription.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ServerDescription);
      }
      if (AuthType != global::Authentication.AuthTypes.ClientKey) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) AuthType);
      }
      if (SessionId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(SessionId);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(HelloPacket other) {
      if (other == null) {
        return;
      }
      if (other.ServerName.Length != 0) {
        ServerName = other.ServerName;
      }
      if (other.ServerDescription.Length != 0) {
        ServerDescription = other.ServerDescription;
      }
      if (other.AuthType != global::Authentication.AuthTypes.ClientKey) {
        AuthType = other.AuthType;
      }
      if (other.SessionId.Length != 0) {
        SessionId = other.SessionId;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            ServerName = input.ReadString();
            break;
          }
          case 18: {
            ServerDescription = input.ReadString();
            break;
          }
          case 24: {
            AuthType = (global::Authentication.AuthTypes) input.ReadEnum();
            break;
          }
          case 34: {
            SessionId = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
