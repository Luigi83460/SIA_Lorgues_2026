#include <SPI.h>
#include <Ethernet.h>

// --- BROCHES MATÉRIELLES ---
const int pinNiveauEau = A0; // Analogue : lu depuis le DAC0 du LabJack
const int pinEauCanal = 2;   // Numérique : présence eau
const int pinPluie = 3;      // Numérique : pluie
const int pinPompe = 4;      // Sortie : Pompe
const int pinEV1 = 5;        // Sortie : Électrovanne Irrigation
const int pinEV2 = 6;        // Sortie : Électrovanne Vidange

// --- RÉSEAU ---
byte mac[] = { 0x90, 0xA2, 0xDA, 0x00, 0x59, 0x67 };
EthernetClient client;
IPAddress ipArduino(192, 168, 1, 20); // IP fixe de l'Arduino
IPAddress ipServeur(192, 168, 1, 10); // IP fixe du serveur PHP/MySQL

// État mémoire des actionneurs
bool etatPompe = false;
bool etatEV1 = false;
bool etatEV2 = false;

void setup() {
  Serial.begin(9600);
  
  pinMode(pinEauCanal, INPUT);
  pinMode(pinPluie, INPUT);
  pinMode(pinPompe, OUTPUT);
  pinMode(pinEV1, OUTPUT);
  pinMode(pinEV2, OUTPUT);

  // Initialisation Ethernet avec IP fixe pour plus de stabilité locale
  Ethernet.begin(mac, ipArduino);
  delay(1500);
  Serial.println("Arduino SIA Prêt.");
}

void loop() {
  // 1. Lecture des variables physiques
  int niveauEau = analogRead(pinNiveauEau); // Valeur de 0 à 1023
  bool eauCanal = digitalRead(pinEauCanal);
  bool pluie = digitalRead(pinPluie);
  
  // 2. Traitement d'urgence : Coupure si manque d'eau dans le réservoir[cite: 2]
  // Seuil fixé arbitrairement à 200 (environ 20%)
  if (niveauEau < 200 && etatEV1 == true) { 
      digitalWrite(pinEV1, LOW); 
      etatEV1 = false;
      Serial.println("⚠️ URGENCE : Niveau bas. Irrigation stoppée.");
  }

  // 3. Construction et envoi de la trame HTTP vers l'API PHP
  if (client.connect(ipServeur, 80)) {
    client.print("GET /SIA/api_update.php");
    client.print("?niveau="); client.print(niveauEau);
    client.print("&canal="); client.print(eauCanal);
    client.print("&pluie="); client.print(pluie);
    client.print("&pompe="); client.print(etatPompe);
    client.print("&ev1="); client.print(etatEV1);
    client.print("&ev2="); client.print(etatEV2);
    client.println(" HTTP/1.1");
    client.println("Host: 192.168.1.10");
    client.println("Connection: close");
    client.println();
    client.stop();
    Serial.println("Données envoyées au serveur.");
  } else {
    Serial.println("Erreur de connexion HTTP.");
  }
  
  delay(5000); // Boucle toutes les 5 secondes
}