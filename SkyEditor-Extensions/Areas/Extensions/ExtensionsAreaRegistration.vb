Imports System.Web.Mvc

Namespace Areas.Extensions
    Public Class ExtensionsAreaRegistration
        Inherits AreaRegistration

        Public Overrides ReadOnly Property AreaName() As String
            Get
                Return "Extensions"
            End Get
        End Property

        Public Overrides Sub RegisterArea(ByVal context As AreaRegistrationContext)
            context.MapRoute(
                "Extensions_default",
                "Extensions/{controller}/{action}/{id}",
                New With {.action = "Index", .id = UrlParameter.Optional}
            )
        End Sub
    End Class
End Namespace