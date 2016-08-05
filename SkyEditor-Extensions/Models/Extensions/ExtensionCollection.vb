Imports System.ComponentModel.DataAnnotations

Namespace Models.Extensions
    Public Class ExtensionCollection
        <Required> <Key> Public Property ID As Guid
        Public Property ParentCollectionId As Guid?
        <Required> Public Property Name As String

        Public Overridable Property ParentCollection As ExtensionCollection
    End Class
End Namespace

