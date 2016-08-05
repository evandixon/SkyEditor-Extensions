Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models.Extensions
    Public Class ExtensionVersion
        <Required> <Key> <Column(Order:=0)> Public Property ID As Guid
        <Required> <Key> <Column(Order:=1)> Public Property Version As String
        <Required> Public Property ExtensionId As Guid

        Public Overridable Property Extension As Extension
    End Class
End Namespace

