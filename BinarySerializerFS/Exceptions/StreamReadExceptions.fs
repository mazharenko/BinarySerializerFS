module BinarySerializerFS.Exceptions

exception StreamEndReachedException

exception UnexpectedStreamEndException of string

let UnexpectedStreamEndException() = 
    UnexpectedStreamEndException "Unexpected end of stream has been detected"