Imports Owin

Partial Public Class Startup
    Public Sub Configuration(app As IAppBuilder)
        ConfigureAuth(app)
        StartExtensionMonitor()
    End Sub
End Class
