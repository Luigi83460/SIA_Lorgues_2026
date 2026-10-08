<?php
// Interface web de consultation de l'état du système

$pdo = new PDO('mysql:host=localhost;dbname=sia_db;charset=utf8', 'root', '');
// On récupère uniquement la dernière ligne enregistrée (la plus récente)
$stmt = $pdo->query('SELECT * FROM etat_systeme ORDER BY date_maj DESC LIMIT 1');
$etatActuel = $stmt->fetch(PDO::FETCH_ASSOC);

// Calcul du pourcentage d'eau (1023 = 100% plein)
$pourcentageEau = $etatActuel ? round(($etatActuel['niveau_eau_reservoir'] / 1023) * 100) : 0;
?>
<!DOCTYPE html>
<html lang="fr">
<head>
    <meta charset="UTF-8">
    <title>SIA - État du Système</title>
    <style>
        body { font-family: 'Segoe UI', sans-serif; background: #eef2f3; padding: 20px; }
        .dashboard { background: white; padding: 25px; border-radius: 10px; width: 400px; box-shadow: 0 5px 15px rgba(0,0,0,0.1); }
        .danger { color: #e74c3c; font-weight: bold; }
        .success { color: #2ecc71; font-weight: bold; }
    </style>
</head>
<body>
    <div class="dashboard">
        <h2>📡 Moniteur SIA</h2>
        <?php if ($etatActuel): ?>
            <p>🕒 MàJ : <?= $etatActuel['date_maj'] ?></p>
            <p>💧 Réservoir : <?= $pourcentageEau ?>% 
               <?php if($pourcentageEau < 20) echo "<span class='danger'>(Critique)</span>"; ?>
            </p>
            <p>🌊 Canal : <?= $etatActuel['presence_eau_canal'] ? "<span class='success'>OK</span>" : "<span class='danger'>Vide</span>" ?></p>
            <p>⚙️ Pompe : <?= $etatActuel['etat_pompe'] ? "ON" : "OFF" ?></p>
            <p>🌱 Arrosage (EV1) : <?= $etatActuel['etat_electrovanne_1'] ? "ON" : "OFF" ?></p>
        <?php else: ?>
            <p>En attente de connexion Arduino...</p>
        <?php endif; ?>
    </div>
</body>
</html>