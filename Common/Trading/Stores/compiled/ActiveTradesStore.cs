// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Stores/ActiveTradesStore.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Trading {

  /// <summary>Holder for reflection information generated from Stores/ActiveTradesStore.proto</summary>
  public static partial class ActiveTradesStoreReflection {

    #region Descriptor
    /// <summary>File descriptor for Stores/ActiveTradesStore.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ActiveTradesStoreReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch5TdG9yZXMvQWN0aXZlVHJhZGVzU3RvcmUucHJvdG8SB1RyYWRpbmcaF1N0",
            "b3Jlcy9UcmFkZVN0b3JlLnByb3RvIjgKEUFjdGl2ZVRyYWRlc1N0b3JlEiMK",
            "BlRyYWRlcxgBIAMoCzITLlRyYWRpbmcuVHJhZGVTdG9yZWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Trading.TradeStoreReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Trading.ActiveTradesStore), global::Trading.ActiveTradesStore.Parser, new[]{ "Trades" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ActiveTradesStore : pb::IMessage<ActiveTradesStore> {
    private static readonly pb::MessageParser<ActiveTradesStore> _parser = new pb::MessageParser<ActiveTradesStore>(() => new ActiveTradesStore());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ActiveTradesStore> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Trading.ActiveTradesStoreReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActiveTradesStore() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActiveTradesStore(ActiveTradesStore other) : this() {
      trades_ = other.trades_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ActiveTradesStore Clone() {
      return new ActiveTradesStore(this);
    }

    /// <summary>Field number for the "Trades" field.</summary>
    public const int TradesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Trading.TradeStore> _repeated_trades_codec
        = pb::FieldCodec.ForMessage(10, global::Trading.TradeStore.Parser);
    private readonly pbc::RepeatedField<global::Trading.TradeStore> trades_ = new pbc::RepeatedField<global::Trading.TradeStore>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Trading.TradeStore> Trades {
      get { return trades_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ActiveTradesStore);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ActiveTradesStore other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!trades_.Equals(other.trades_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= trades_.GetHashCode();
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
      trades_.WriteTo(output, _repeated_trades_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += trades_.CalculateSize(_repeated_trades_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ActiveTradesStore other) {
      if (other == null) {
        return;
      }
      trades_.Add(other.trades_);
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
            trades_.AddEntriesFrom(input, _repeated_trades_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
