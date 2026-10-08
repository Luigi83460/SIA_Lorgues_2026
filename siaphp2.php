<?php
// Script silencieux appelé par l'Arduino pour insérer les données capteurs dans la BDD

$dsn = "mysql:host=127.0.0.1;dbname=sia_db;charset=utf8mb4";
try {
     $pdo = new PDO($dsn, 'root', ''); // Connexion BDD locale (sans mot de passe par défaut)
} catch (\PDOException $e) {
     die('Erreur BDD'); 
}

// Vérifie si l'Arduino a bien envoyé les paramètres vitaux dans l'URL
if(isset($_GET['niveau']) && isset($_GET['canal'])) {
    
    // Requête préparée pour insérer une nouvelle ligne d'état
    $sql = "INSERT INTO etat_systeme 
            (niveau_eau_reservoir, presence_eau_canal, pluie_detectee, etat_pompe, etat_electrovanne_1, etat_electrovanne_2) 
            VALUES (?, ?, ?, ?, ?, ?)";
            
    $stmt = $pdo->prepare($sql);
    
    // Sécurisation (intval) et exécution avec les données de l'URL ($_GET)
    $stmt->execute([
        intval($_GET['niveau']),
        intval($_GET['canal']),
        isset($_GET['pluie']) ? intval($_GET['pluie']) : 0,
        isset($_GET['pompe']) ? intval($_GET['pompe']) : 0,
        isset($_GET['ev1']) ? intval($_GET['ev1']) : 0,
        isset($_GET['ev2']) ? intval($_GET['ev2']) : 0
    ]);
    echo "OK_DATA_SAVED";
}
?>