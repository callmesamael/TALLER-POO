using System;
using System.Collections.Generic;

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

    // Adaptación de AplicarDescuento con la validación exigida
    public void RebajarTarifa(double porcentaje)
    {
        if (porcentaje > 0 && porcentaje <= 100)
        {
            double descuento = TarifaNocturna * (porcentaje / 100);
            double nuevoPrecio = TarifaNocturna - descuento;
            
            if (nuevoPrecio > 0)
            {
                TarifaNocturna = nuevoPrecio;
            }
            else
            {
                Console.WriteLine($"[Aviso] El descuento del {porcentaje}% dejaría la tarifa en 0 o negativo. Operación cancelada para el cuarto {NumeroHabitacion}.");
            }
        }
    }

    // Adaptación de ObtenerDatosResumen con el formato solicitado
    public string GenerarResumenCorto()
    {
        string estado = EstaLibre ? "Libre" : $"Ocupada por: {NombreHuesped}";
        return $"[{NumeroHabitacion}] {Categoria} - ${TarifaNocturna:N0} - {estado}";
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

        Console.WriteLine("*** INICIANDO SIMULACIÓN DE RESERVAS ***");

        cuarto1.AgendarEstadia("Martin Elias", 3);
        cuarto2.AgendarEstadia("Ana Maria", 2);
        cuarto1.DesocuparCuarto();
        cuarto3.RebajarTarifa(10);
        cuarto1.AgendarEstadia("Santiago", 1);
        
        Console.WriteLine("Simulación completada con éxito.\n");

        // Implementación del ciclo foreach para métricas (Requisito 2A y 2B)
        int cuartosLibres = 0;
        int cuartosOcupados = 0;
        double sumaTarifas = 0;

        Console.WriteLine("*** REPORTE DE HABITACIONES ***");
        
        foreach (var cuarto in listaCuartos)
        {
            // Imprimir el formato corto (Requisito 1B)
            Console.WriteLine(cuarto.GenerarResumenCorto());

            // Contar libres y ocupadas
            if (cuarto.EstaLibre)
            {
                cuartosLibres++;
            }
            else
            {
                cuartosOcupados++;
            }

            // Sumar tarifas para el promedio final
            sumaTarifas += cuarto.TarifaNocturna;
        }

        // Calcular el promedio (Requisito 2B)
        double tarifaPromedio = sumaTarifas / listaCuartos.Count;

        Console.WriteLine("\n*** MÉTRICAS FINALES DEL HOTEL ***");
        Console.WriteLine($"- Habitaciones Libres:   {cuartosLibres}");
        Console.WriteLine($"- Habitaciones Ocupadas: {cuartosOcupados}");
        Console.WriteLine($"- Tarifa Promedio:       ${tarifaPromedio:N0}");
    }
}
