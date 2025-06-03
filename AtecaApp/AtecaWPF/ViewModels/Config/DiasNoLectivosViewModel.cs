using CsvHelper;
using System.Globalization;
using System.IO;


public partial class DiasNoLectivosViewModel : ViewModel
{
    private readonly HttpJsonClient _httpJsonClient;

    [ObservableProperty]
    private ObservableCollection<DiaNoLectivoDTO> diasNoLectivos = new();

    [ObservableProperty]
    private DiaNoLectivoDTO? diaSeleccionado;

    [ObservableProperty]
    private DateOnly? nuevaFechaNoLectiva;

    public DateTime? NuevaFechaNoLectivaDateTime // Propiedad para formatear el tipo devuelto del DatePicker
    {
        get => NuevaFechaNoLectiva?.ToDateTime(TimeOnly.MinValue);
        set => NuevaFechaNoLectiva = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
    }


    public DiasNoLectivosViewModel(HttpJsonClient httpJsonClient)
    {
        _httpJsonClient = httpJsonClient;
    }

    [RelayCommand]
    public async Task CargarDiasNoLectivosAsync()
    {
        try
        {
            var lista = await _httpJsonClient.GetListAsync<DiaNoLectivoDTO>("api/DiaNoLectivo");
            DiasNoLectivos = new ObservableCollection<DiaNoLectivoDTO>(lista);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error cargando días no lectivos: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task AddDiaNoLectivoAsync()
    {
        if (NuevaFechaNoLectiva == null)
        {
            MessageBox.Show("Seleccione una fecha válida.");
            return;
        }

        var nuevoDia = new DiaNoLectivoDTO
        {
            Fecha = NuevaFechaNoLectiva.Value
        };

        await GuardarDiaNoLectivoAsync(nuevoDia);
        NuevaFechaNoLectiva = null;
    }

    [RelayCommand]
    public async Task GuardarDiaNoLectivoAsync(DiaNoLectivoDTO dia)
    {
        try
        {
            if (dia.Id == 0)
            {
                await _httpJsonClient.PostAsync<DiaNoLectivoDTO, DiaNoLectivoDTO>("api/DiaNoLectivo", dia);
            }
            else
            {
                await _httpJsonClient.PutAsync($"api/DiaNoLectivo/{dia.Id}", dia);
            }
            await CargarDiasNoLectivosAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error guardando día no lectivo: {ex.Message}");
        }
    }



    [RelayCommand]
    public async Task EliminarDiaNoLectivoAsync(int id)
    {
        try
        {
            await _httpJsonClient.DeleteAsync($"api/DiaNoLectivo/{id}");
            await CargarDiasNoLectivosAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error eliminando día no lectivo: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task ImportarDesdeCsvAsync()
    {
        var openFileDialog = new Microsoft.Win32.OpenFileDialog
        {
            Filter = "CSV Files (*.csv)|*.csv",
            Title = "Selecciona un archivo CSV"
        };

        if (openFileDialog.ShowDialog() == true)
        {
            try
            {
                using var reader = new StreamReader(openFileDialog.FileName);
                using var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture);

                // Línea para leer la cabecera
                await csv.ReadAsync();
                csv.ReadHeader();

                var registros = new List<CreateDiaNoLectivoDTO>();

                while (await csv.ReadAsync())
                {
                    var fechaTexto = csv.GetField("Fecha");
                    if (DateOnly.TryParse(fechaTexto, out var fecha))
                    {
                        registros.Add(new CreateDiaNoLectivoDTO { Fecha = fecha });
                    }
                }

                foreach (var registro in registros)
                {
                    await GuardarDiaNoLectivoAsync(new DiaNoLectivoDTO { Fecha = registro.Fecha });
                }

                MessageBox.Show("Importación completada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al importar CSV: {ex.Message}");
            }
        }
    }



}
