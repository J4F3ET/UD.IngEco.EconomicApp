namespace EconomicApp.Models.DTOs;

public enum Frecuencia
{
    Mensual = 1,
    Bimestral = 2,
    Trimestral = 3,
    Semestral = 6,
    Anual = 12
}

public enum TipoTasa
{
    Nominal,
    Efectiva
}

public enum SistemaAmortizacion
{
    Frances,
    Aleman
}

public enum FormatoExportacion
{
    PDF,
    Excel
}