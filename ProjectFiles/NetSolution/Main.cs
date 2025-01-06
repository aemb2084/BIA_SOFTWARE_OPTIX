#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.NetLogic;
using FTOptix.CoreBase;
using FTOptix.Core;
using System.Threading;
using System.Threading.Tasks;
#endregion

public class Main : BaseNetLogic
{
    private Int32 Id;
    private CancellationTokenSource cancellationTokenSource;
    private CancellationTokenSource cancellationTokenSource1;
    private CancellationTokenSource cancellationTokenSource2;
    public override void Start()
    {

       // StartLoops();

        //StopLoops();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }

    [ExportMethod]
    public void Method1()
    {
        // Insert code to be executed by the method
        Id = Project.Current.GetVariable("Model/id").Value;

        cancellationTokenSource1 = new CancellationTokenSource();
        cancellationTokenSource2 = new CancellationTokenSource();

        switch (Id)
        {
            case 1:
                _ = RunLoopAsync("Loop 4", 3000, cancellationTokenSource1.Token, cancellationTokenSource1);
                break;
            case 2:
                _ = RunLoopAsync("Loop 5", 3000, cancellationTokenSource2.Token, cancellationTokenSource2);
                break;
            default:
                break;
        }

    }

    public void StartLoops()
    {
        cancellationTokenSource = new CancellationTokenSource();

        // Crea e inicia varios loops asíncronos
        _ = RunLoopAsync("Loop 1", 1000, cancellationTokenSource.Token, cancellationTokenSource);
        _ = RunLoopAsync("Loop 2", 2000, cancellationTokenSource.Token, cancellationTokenSource);
        _ = RunLoopAsync("Loop 3", 3000, cancellationTokenSource.Token, cancellationTokenSource);

        Log.Info("Loops iniciados.");
    }

    public void StopLoops(CancellationTokenSource cancellationTokenSource)
    {
        // Cancela todos los loops
        cancellationTokenSource.Cancel();
        Log.Info("Loops detenidos.");
    }

    private async Task RunLoopAsync(string loopName, int delayMilliseconds, CancellationToken cancellationToken, CancellationTokenSource cancellationTokenSource)
    {
        int loops = 0;
        Log.Info($"CancellationToken {cancellationToken.IsCancellationRequested}");
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Log.Info($"{loopName} {loops} ejecutado en: {DateTime.Now}");
                //await Task.Delay(delayMilliseconds); // Espera el tiempo especificado
                loops++;
                await Task.Delay(delayMilliseconds, cancellationToken);
                if(loops >= 5){
                    StopLoops(cancellationTokenSource);
                }
            }
        }
        catch (TaskCanceledException)
        {
            Log.Info($"{loopName} detenido.");
        }
        catch (Exception ex)
        {
            Log.Info($"Error en {loopName}: {ex.Message}");
        }
    }


}
