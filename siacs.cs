using System;
using LabJack.LabJackUD; 

public class SimulateurPartieOperative
{
    private int ljHandle = 0; // Handle USB du boîtier

    /// <summary>
    /// Connecte le logiciel C# au matériel Labjack en USB[cite: 5].
    /// </summary>
    public void DemarrerSimulation()
    {
        try
        {
            // Initialisation de la connexion USB avec le LabJack U3
            LJUD.OpenLabJack(LJUD.DEVICE.U3, LJUD.CONNECTION.USB, "1", true, ref ljHandle);
            Console.WriteLine("✅ LabJack U3 détecté et connecté.");
        }
        catch (LabJackUDException e)
        {
            Console.WriteLine("❌ LabJack non trouvé : " + e.ToString());
        }
    }

    /// <summary>
    /// Génère une tension de 0 à 5V sur la sortie DAC0. 
    /// L'Arduino, branché sur cette broche, l'interprètera comme le niveau d'eau[cite: 2].
    /// </summary>
    public void SimulerNiveauEau(int pourcentage)
    {
        try
        {
            // Borner la saisie entre 0 et 100%
            if (pourcentage < 0) pourcentage = 0;
            if (pourcentage > 100) pourcentage = 100;

            // Conversion du pourcentage en Volts (Ex: 50% = 2.5V)
            double tensionSortie = (pourcentage / 100.0) * 5.0;

            // Commande eDAC : Canal 0 (DAC0), Valeur (tensionSortie)
            LJUD.eDAC(ljHandle, 0, tensionSortie, 0, 0, 0);
            
            Console.WriteLine($"[Simulation] Niveau d'eau réglé sur {pourcentage}% -> Sortie analogique : {tensionSortie}V");
        }
        catch (LabJackUDException e)
        {
            Console.WriteLine("❌ Erreur d'écriture LabJack : " + e.ToString());
        }
    }
}