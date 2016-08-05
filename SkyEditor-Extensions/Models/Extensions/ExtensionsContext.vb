Imports System.Data.Entity

Namespace Models.Extensions
    Public Class ExtensionsContext
        Inherits DbContext
        Public Sub New()
            MyBase.New("Extensions")
        End Sub

        Public Shared Function Create() As ExtensionsContext
            Return New ExtensionsContext()
        End Function

        Public Property ExtensionCollections As DbSet(Of ExtensionCollection)
        Public Property Extensions As DbSet(Of Extension)
        Public Property ExtensionVersions As DbSet(Of ExtensionVersion)
    End Class
End Namespace

