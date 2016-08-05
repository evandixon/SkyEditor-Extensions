Imports System.ComponentModel.DataAnnotations

Namespace Models.Extensions
    Public Class Extension
        <Required> <Key> Public Property ID As Guid
        <Required> Public Property ExtensionCollectionId As Guid
        <Required> Public Property ExtensionID As String
        <Required> Public Property Name As String
        Public Property Description As String
        <Required> Public Property Author As String

        Public Overridable Property ExtensionCollection As ExtensionCollection
    End Class
End Namespace

