
Imports System.ComponentModel
Imports System.Threading
Imports System.Windows.Threading
Imports MongoDB.Bson
Imports Squirrel
Imports LibTradutorNetFramework


Class MainWindow


    Public Traducoes As List(Of String) = ObterTraducoes()
    Public WithEvents Tradutor As New Tradutor



    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        AdicionarNumeroVersao()

        Me.ListaTraducoes.ItemsSource = Traducoes

    End Sub



    Private Sub AdicionarNumeroVersao()
        Dim assembly = System.Reflection.Assembly.GetExecutingAssembly
        Dim versioninfo = FileVersionInfo.GetVersionInfo(assembly.Location)
        Me.Title += $" v.{versioninfo.FileVersion} "

    End Sub


    Private Function ObterTraducoes() As List(Of String)

        Dim x As New LibTradutorNetFramework.MongoDb("TRADUÇÃO")
        Traducoes = x.ObterTraducoesProntas()

        Return Traducoes

    End Function



    Private Sub MostrarLog(sender As Object, text As eventargstring) Handles Tradutor.EnviarLog

        Dispatcher.Invoke(Sub()
                              Me.LOG.Text += Environment.NewLine + text.log + " " + Now.ToShortTimeString
                          End Sub, DispatcherPriority.Background)

    End Sub

    Private Function Obterarquivo() As String

        ' Create OpenFileDialog 
        Dim dlg As Microsoft.Win32.OpenFileDialog = New Microsoft.Win32.OpenFileDialog()

        ' Set filter for file extension And default file extension 
        dlg.DefaultExt = ".wav"
        dlg.Filter = "ALL Files (*.*)|*.*" '"JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif"; 


        ' Display OpenFileDialog by calling ShowDialog method 
        Dim result As Nullable(Of Boolean) = dlg.ShowDialog()


        ' Get the selected file name and display in a TextBox 
        If result = True Then

            ' Open document 
            Dim FilePath As String = dlg.FileName


            Return FilePath

        End If



    End Function

    Private Sub BotaoTraduxzirVideo_Click(sender As Object, e As RoutedEventArgs)

        Dim Filepath As String = Obterarquivo()

        If Filepath IsNot Nothing Then
            DesativarBotoes()


            Dim x As New Thread(Sub() Tradutor.TraduzirVideo(Filepath))
            x.Start()

        End If
    End Sub

    Private Sub BotaoTraduzirAudio_Click(sender As Object, e As RoutedEventArgs)

        Dim Filepath As String = Obterarquivo()

        If Filepath IsNot Nothing Then
            DesativarBotoes()
            Dim X As New Thread(Sub() Tradutor.traduzirPorBlocos(Filepath))
            X.Start()
        End If

    End Sub

    Private Sub BotaoResgatarTraducao_Click(sender As Object, e As RoutedEventArgs)

        DesativarBotoes()

        Tradutor.resgatarTraducao(CType(ListaTraducoes.SelectedItem, String))

    End Sub

    Private Sub DesativarBotoes()
        BotaoResgatarTraducao.IsEnabled = False
        BotaoTraduxzirVideo.IsEnabled = False
        BotaoTraduzirAudio.IsEnabled = False

    End Sub


    Private Sub ReativarBotoes() Handles Tradutor.Finalizado
        Dispatcher.Invoke(Sub()

                              BotaoResgatarTraducao.IsEnabled = True
                              BotaoTraduxzirVideo.IsEnabled = True
                              BotaoTraduzirAudio.IsEnabled = True

                              Traducoes = ObterTraducoes()
                              ListaTraducoes.ItemsSource = Traducoes
                          End Sub)
    End Sub

    Private Sub MostrarErro(sender As Object, text As eventargstring) Handles Tradutor.EnviarErro

        Dispatcher.Invoke(Sub()
                              MsgBox(text.log)
                          End Sub, DispatcherPriority.Background)

    End Sub
End Class
