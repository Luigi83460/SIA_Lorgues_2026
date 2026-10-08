using System;
using MySql.Data.MySqlClient; 

public class PosteResponsable
{
    // L'IP pointe vers le PC Serveur WAMP/XAMPP
    private string connectionString = "Server=192.168.1.10;Database=sia_db;Uid=root;Pwd=;";

    /// <summary>
    /// Ajoute un programme d'irrigation défini par le responsable[cite: 4, 7].
    /// </summary>
    public void AjouterProgramme(string parcelle, string heureDebut, int dureeMinutes)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string query = "INSERT INTO programmes (parcelle, heure_debut, duree_minutes) VALUES (@parc, @heure, @duree)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@parc", parcelle);
                    cmd.Parameters.AddWithValue("@heure", heureDebut);
                    cmd.Parameters.AddWithValue("@duree", dureeMinutes);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("✅ Programme d'arrosage enregistré.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Erreur MySQL : " + ex.Message);
            }
        }
    }

    /// <summary>
    /// Permet de basculer du mode Automatique au mode Manuel[cite: 4].
    /// </summary>
    public void ChangerModeSysteme(bool activerModeManuel)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();
            // On met à jour la ligne de configuration unique (ID 1)
            string query = "UPDATE configuration SET mode_manuel = @mode WHERE id = 1";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@mode", activerModeManuel);
                cmd.ExecuteNonQuery();
                Console.WriteLine(activerModeManuel ? "🛠️ Mode Manuel Activé" : "🤖 Mode Auto Activé");
            }
        }
    }
}