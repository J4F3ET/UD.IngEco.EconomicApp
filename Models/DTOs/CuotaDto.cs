namespace EconomicApp.Models.DTOs;

public class CuotaDto
{
    public int NumeroPeriodo { get; set; }
    public decimal SaldoInicial { get; set; }
    public decimal Interes { get; set; }
    public decimal AbonoCapital { get; set; }
    public decimal ValorCuota { get; set; }
    public decimal SaldoFinal { get; set; }
}