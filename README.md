# 📚 Documentation API - Gestion de Parc

## 🧩 Présentation Générale

Cette API a été développée pour centraliser la gestion d’un parc informatique.  
Elle permet de gérer différents types de ressources :

- Postes de travail
- Salles
- Parcs
- Incidents
- Actions

Un système d’authentification via **JWT** sécurise l’accès aux différentes opérations.

L’API est structurée en plusieurs **contrôleurs (controllers)**, chacun dédié à une ressource spécifique.  
Elle est utilisée par :

- Une interface **Web Blazor** (utilisateur)
- Une application **mobile MAUI** (utilisateur - mobilité)
- Une application **desktop WPF** et un **service Windows** (machine)

---

## 🔐 Authentification

- Un controller dédié permet à un utilisateur de se connecter.
- Un **token JWT** est généré lors de la connexion.
- Ce token est requis pour la majorité des endpoints protégés.
- Certains endpoints sont **publics**, notamment ceux destinés aux **postes de travail (devices)** pour remonter automatiquement des informations via le **service Windows**.

---

## 🗂️ Ressources disponibles

### 🏢 Parcs (`ParkController`)

| Méthode | Endpoint            | Description                          |
|--------|---------------------|--------------------------------------|
| GET    | `/parks`            | Récupère la liste de tous les parcs |
| GET    | `/parks/{id}`       | Récupère un parc spécifique          |
| POST   | `/parks`            | Crée un nouveau parc                 |
| PUT    | `/parks/{id}`       | Modifie un parc existant             |
| DELETE | `/parks/{id}`       | Supprime un parc                     |

---

### 🏬 Salles (`RoomController`)

| Méthode | Endpoint            | Description                          |
|--------|---------------------|--------------------------------------|
| GET    | `/rooms`            | Liste toutes les salles              |
| GET    | `/rooms/{id}`       | Détail d’une salle                   |
| POST   | `/rooms`            | Ajoute une salle                     |
| PUT    | `/rooms/{id}`       | Met à jour une salle                 |
| DELETE | `/rooms/{id}`       | Supprime une salle                   |

---

### 💻 Postes de travail (`DeviceController`)

| Méthode | Endpoint                      | Description                                                                 |
|--------|-------------------------------|-----------------------------------------------------------------------------|
| GET    | `/devices`                    | Liste tous les postes enregistrés (**protégé**)                            |
| GET    | `/devices/{id}`               | Détail d’un poste (**protégé**)                                            |
| POST   | `/devices`                    | Ajoute un nouveau poste (**non protégé**, utilisé par l'app WPF)           |
| PUT    | `/devices/{id}`               | Met à jour un poste (**protégé**)                                          |
| PUT    | `/devices/mac{macAddress}`    | Met à jour les infos du poste (**non protégé**, utilisé par le service)   |
| DELETE | `/devices/{id}`               | Supprime un poste (**protégé**)                                            |

> `PUT /devices/mac{macAddress}` est **non protégé**, utilisé par le service Windows pour mettre à jour automatiquement les données des postes.

---

### 🛠️ Incidents (`IncidentController`)

| Méthode | Endpoint            | Description                           |
|--------|---------------------|---------------------------------------|
| GET    | `/incidents`        | Liste des incidents déclarés          |
| GET    | `/incidents/{id}`   | Détail d’un incident                  |
| POST   | `/incidents`        | Crée un nouvel incident (ex: panne)   |
| PUT    | `/incidents/{id}`   | Met à jour un incident                |
| DELETE | `/incidents/{id}`   | Supprime un incident                  |

---

### 🔄 Actions (`ActionController`)

| Méthode | Endpoint            | Description                                                                 |
|--------|---------------------|-----------------------------------------------------------------------------|
| GET    | `/actions`          | Liste toutes les actions liées aux incidents ou postes                      |
| GET    | `/actions/{id}`     | Détail d’une action                                                         |
| POST   | `/actions`          | Crée une action (ex: redémarrage, verrouillage, etc.)                      |
| PATCH  | `/actions/{id}`     | Met à jour l’état d’une action (**non protégé**) – utilisé par le service  |
| DELETE | `/actions/{id}`     | Supprime une action                                                         |

> `PATCH /actions/{id}` est **non protégé**, utilisé par le **service Windows** pour signaler qu’une action a été exécutée localement sur un poste.

---

## 🧠 Origine des données

Les **postes de travail (devices)** ne sont pas renseignés manuellement.  
Ils sont détectés et mis à jour automatiquement par :

- Un **service Windows** tournant en arrière-plan sur chaque machine.
- Une **application WPF** installée sur les postes.

Ces outils interagissent avec l’API via les endpoints **publics** :

- `POST /devices` → Crée ou met à jour une fiche device.
- `PUT /devices/mac{macAddress}` → Met à jour les informations spécifiques.
- `PATCH /actions/{id}` → Informe l’API qu’une action locale a été exécutée.

---
