

public partial class FranjasViewModel : ViewModel
{
    private readonly HttpJsonClient _httpJsonClient;

    [ObservableProperty]
    private ObservableCollection<FranjaHorariaDTO> franjas = new();

    [ObservableProperty]
    private FranjaHorariaDTO? franjaSeleccionada;

    [ObservableProperty]
    private string nuevaHoraInicioTexto = string.Empty;

    [ObservableProperty]
    private string nuevaHoraFinTexto = string.Empty;

    public FranjasViewModel(HttpJsonClient httpJsonClient)
    {
        _httpJsonClient = httpJsonClient;
    }

    [RelayCommand]
    public async Task CargarFranjasAsync()
    {
        try
        {
            var lista = await _httpJsonClient.GetListAsync<FranjaHorariaDTO>("api/FranjaHoraria");
            Franjas = new ObservableCollection<FranjaHorariaDTO>(lista);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error cargando franjas: {ex.Message}");
        }
    }

    [RelayCommand]
    public async Task AddFranjaAsync()
    {
        if (!TimeOnly.TryParse(NuevaHoraInicioTexto, out var inicio))
        {
            MessageBox.Show("Hora de inicio inválida (formato esperado: HH:mm)");
            return;
        }

        if (!TimeOnly.TryParse(NuevaHoraFinTexto, out var fin))
        {
            MessageBox.Show("Hora de fin inválida (formato esperado: HH:mm)");
            return;
        }

        var nuevaFranja = new FranjaHorariaDTO
        {
            HoraInicio = inicio,
            HoraFin = fin,
            Activo = true
        };

        await GuardarFranjaAsync(nuevaFranja);

        NuevaHoraInicioTexto = string.Empty;
        NuevaHoraFinTexto = string.Empty;
    }

    [RelayCommand]
    public async Task GuardarFranjaAsync(FranjaHorariaDTO franja)
    {
        try
        {
            if (!TimeOnly.TryParse(NuevaHoraInicioTexto, out var inicio) ||
                !TimeOnly.TryParse(NuevaHoraFinTexto, out var fin))
            {
                MessageBox.Show("Formato de hora inválido. Use HH:mm.");
                return;
            }

            franja.HoraInicio = inicio;
            franja.HoraFin = fin;

            if (franja.Id == 0)
            {
                await _httpJsonClient.PostAsync<FranjaHorariaDTO, FranjaHorariaDTO>("api/FranjaHoraria", franja);
            }
            else
            {
                await _httpJsonClient.PutAsync($"api/FranjaHoraria/{franja.Id}", franja);
            }

            await CargarFranjasAsync();

            // Limpieza opcional
            NuevaHoraInicioTexto = string.Empty;
            NuevaHoraFinTexto = string.Empty;
            FranjaSeleccionada = null;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error guardando franja: {ex.Message}");
        }
    }


    [RelayCommand]
    public async Task EliminarFranjaAsync(int id)
    {
        try
        {
            await _httpJsonClient.DeleteAsync($"api/FranjaHoraria/{id}");
            await CargarFranjasAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error eliminando franja: {ex.Message}");
        }
    }

    partial void OnFranjaSeleccionadaChanged(FranjaHorariaDTO? value)
    {
        if (value != null)
        {
            NuevaHoraInicioTexto = value.HoraInicio.ToString("HH:mm");
            NuevaHoraFinTexto = value.HoraFin.ToString("HH:mm");
        }
    }

}
