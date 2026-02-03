@echo off
echo ==========================================
echo    DEMARRAGE DE L'ENVIRONNEMENT DE DEV
echo ==========================================

echo 1. Lancement de l'API (Serveur)...
start "SERVEUR API" cmd /c "cd /d \"C:\Users\oussa\OneDrive\Desktop\Formation\Projet full stack MVC\Formation-Ecommerce-11-2025\Formation-Ecommerce-11-2025\" && dotnet run"

echo Attente du demarrage de l'API (10s)...
timeout /t 10

echo 2. Lancement du Client MVC...
echo L'application sera accessible sur https://localhost:5002
cd /d "C:\Users\oussa\OneDrive\Desktop\Formation\Projet extensions\Projet CLient MVC\Formation-Ecommerce-Client"
dotnet run
