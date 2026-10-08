-- Création de la base de données principale
CREATE DATABASE IF NOT EXISTS sia_db;
USE sia_db;

-- Table de configuration pour le mode de fonctionnement (Automatique / Manuel)[cite: 4]
CREATE TABLE configuration (
    id INT PRIMARY KEY AUTO_INCREMENT,
    heure_remplissage TIME NOT NULL DEFAULT '08:00:00',
    mode_manuel BOOLEAN DEFAULT FALSE 
);

-- Insertion de la configuration par défaut (Ligne ID 1)
INSERT INTO configuration (heure_remplissage, mode_manuel) VALUES ('08:00:00', 0);

-- Table pour les programmes d'arrosage créés par le responsable
CREATE TABLE programmes (
    id INT PRIMARY KEY AUTO_INCREMENT,
    parcelle VARCHAR(50) NOT NULL,
    heure_debut TIME NOT NULL,
    duree_minutes INT NOT NULL
);

-- Table d'historique de l'état du système (alimentée par l'Arduino)
CREATE TABLE etat_systeme (
    id INT PRIMARY KEY AUTO_INCREMENT,
    niveau_eau_reservoir INT,          -- Valeur lue du Labjack (0-1023)
    presence_eau_canal BOOLEAN,        -- Capteur TOR
    pluie_detectee BOOLEAN,            -- Capteur TOR
    etat_pompe BOOLEAN,                -- Pompe de remplissage
    etat_electrovanne_1 BOOLEAN,       -- Circuit irrigation
    etat_electrovanne_2 BOOLEAN,       -- Circuit vidange
    date_maj DATETIME DEFAULT CURRENT_TIMESTAMP
);