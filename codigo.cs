using System;
using System.Collections.Generic;
using System.Linq;

class Cuarto
{
    private string categoria;
    private double tarifaNocturna;

    public int NumeroHabitacion { get; }
    public bool EstaLibre { get; private set; }
    public string NombreHuesped { get; private set; }

    public string Categoria
    {
        get => categoria;
        set
        {
            string tipoLower = value.ToLower();
            if (tipoLower == "sencilla" || tipoLower == "doble" || tipoLower == "premium")
            {
                categoria = value;
            }
            else
            {
                throw new ArgumentException("La categoría debe ser Sencilla, Doble o Premium.");
            }
        }
    }

    public double TarifaNocturna
    {
        get => tarifaNocturna;
        set
        {
            if (value <= 0) throw new ArgumentException("La tarifa debe ser un valor positivo.");
            tarifaNocturna = value;
        }
    }

    public Cuarto(int numero, string tipo, double tarifa)
    {
        NumeroHabitacion = numero;
        Categoria = tipo;
        TarifaNocturna = tarifa;
        EstaLibre = true;
        NombreHuesped = string.Empty;
    }

    public double AgendarEstadia(string huesped, int dias)
    {
        if (!EstaLibre) return -1;

        EstaLibre = false;
        NombreHuesped = huesped;
        return TarifaNocturna * dias;
    }

    public bool DesocuparCuarto()
    {
        if (EstaLibre) return false;

        EstaLibre = true;
        NombreHuesped = string.Empty;
        return true;
    }

    public void RebajarTarifa(double porcentaje)
    {
        if (porcentaje > 0 && porcentaje <= 100)
        {
            TarifaNocturna -= TarifaNocturna * (porcentaje / 100);
        }
    }

    public string ObtenerDetalles()
    {
        string estado = EstaLibre ? "Libre" : $"Ocupado por {NombreHuesped}";
        return $"Cuarto {NumeroHabitacion} | Categoría: {Categoria} | Tarifa: ${TarifaNocturna:N0} | Estatus: {estado}";
    }
}

class Program
{
    static void Main(string[] args)
    {
        var cuarto1 = new Cuarto(101, "Sencilla", 50000);
        var cuarto2 = new Cuarto(202, "Doble", 80000);
        var cuarto3 = new Cuarto(303, "Premium", 150000);

        var listaCuartos = new List<Cuarto> { cuarto1, cuarto2, cuarto3 };

        Console.WriteLine("*** REPORTE INICIAL ***");
        listaCuartos.ForEach(c => Console.WriteLine(c.ObtenerDetalles()));
        Console.WriteLine();

        Console.WriteLine("*** INICIANDO OPERACIONES ***");

        double pago1 = cuarto1.AgendarEstadia("Martin Elias", 3);
        Console.WriteLine($"- Se agendó el cuarto 101 para Martin Elias por 3 días. A pagar: ${pago1:N0}");

        double pago2 = cuarto2.AgendarEstadia("Ana Maria", 2);
        Console.WriteLine($"- Se agendó el cuarto 202 para Ana Maria por 2 días. A pagar: ${pago2:N0}");

        double validacionOcupado = cuarto1.AgendarEstadia("Pedro Perez", 1);
        if (validacionOcupado == -1)
        {
            Console.WriteLine("- Intento de reserva en cuarto 101: Denegado (Cuarto no disponible).");
        }

        bool checkout = cuarto1.DesocuparCuarto();
        Console.WriteLine($"- Proceso de salida cuarto 101: {(checkout ? "Completado" : "El cuarto ya estaba vacío")}");

        cuarto3.RebajarTarifa(10);
        Console.WriteLine("- Descuento del 10% aplicado al cuarto 303 (Premium).");

        double pago3 = cuarto1.AgendarEstadia("Santiago", 1);
        Console.WriteLine($"- Se agendó el cuarto 101 para Santiago por 1 día. A pagar: ${pago3:N0}");
        Console.WriteLine();

        Console.WriteLine("*** REPORTE FINAL ***");
        foreach (var cuarto in listaCuartos)
        {
            Console.WriteLine(cuarto.ObtenerDetalles());
        }
        Console.WriteLine();

        int cuartosLlenos = listaCuartos.Count(c => !c.EstaLibre);
        Console.WriteLine("*** ESTADÍSTICAS ***");
        Console.WriteLine($"Total de cuartos ocupados actualmente: {cuartosLlenos}");
    }
}
