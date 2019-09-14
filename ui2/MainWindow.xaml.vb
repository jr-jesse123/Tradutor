
Imports System.ComponentModel
Imports System.Threading
Imports System.Windows.Threading
Imports LibTradutorNetFramework


Class MainWindow


    Public Traducoes As List(Of String) = ObterTraducoes()
    Public WithEvents Tradutor As New Tradutor


    Sub New()

        ' Esta chamada é requerida pelo designer.
        InitializeComponent()

        ' Adicione qualquer inicialização após a chamada InitializeComponent().

        Me.ListaTraducoes.ItemsSource = Traducoes



    End Sub



    Private Function ObterTraducoes() As List(Of String)

        Dim x As New LibTradutorNetFramework.MongoDb("TRADUÇÃO")
        Traducoes = x.ObterTraducoesProntas()

        Dim output As New List(Of String)
        For Each traducao In Traducoes
            Try
                Dim a = IO.Path.GetFileNameWithoutExtension(traducao)
                output.Add(a)
            Catch ex As Exception

            End Try
        Next

        Return output


        Return Traducoes

    End Function

    Private Sub MostrarLog(sender As Object, text As eventargstring) Handles Tradutor.EnviarLog

        Dispatcher.Invoke(Sub()
                              Me.LOG.Text = text.log
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
                              EProgressBar.Value = 0

                              LOG.Text = "Finalizado"

                          End Sub)
    End Sub

    Private Sub MostrarErro(sender As Object, text As eventargstring) Handles Tradutor.EnviarErro

        Dispatcher.Invoke(Sub()
                              MsgBox(text.log)
                          End Sub, DispatcherPriority.Background)

    End Sub

    Private Sub ProgressUpdate(sender As Object, porcentagem As Integer) Handles Tradutor.Progresso

        Dispatcher.Invoke(Sub()
                              EProgressBar.Value = porcentagem
                          End Sub, DispatcherPriority.Background)

    End Sub

End Class
