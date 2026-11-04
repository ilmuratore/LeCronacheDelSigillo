# Le Cronache del Sigillo — Versione 2.0 (EXTRA)

Questa distribuzione **sostituisce la parte tecnica degli Step 26–30 con due guide EXTRA**. Si basa sulle meccaniche degli Step 01–25 ed estende lo Step 26 esistente.

## Avvio

Requisito: **.NET SDK 8**. Apri un terminale nella cartella `LeCronacheDelSigillo` (la cartella che contiene `.csproj`):

```powershell
dotnet build
dotnet run
```

Controlla i contenuti esterni prima dell'avvio:

```powershell
dotnet run -- --validate
dotnet run -- --list
dotnet run -- --demo
```

Con `--validate` il runner verifica l'integrità dei riferimenti. Non crea codice sorgente. I dati vengono caricati da `Dati/*.json` e copiati automaticamente nella directory di output alla build.

## Due guide EXTRA, 120 minuti complessivi

- **EXTRA 1 — 50 min:** PvP, console, punti abilità (+1 per passaggio di livello), JSON persistente.
- **EXTRA 2 — 70 min:** refactoring, collezioni, catalogo, 8 file JSON, nuovi nemici/dialoghi/missioni e runner.

Si utilizza il **progetto già pronto**. Le guide prevedono modifiche molto circoscritte, non la costruzione completa da zero.

## Organizzazione

- `Core`: gestione partita, menu, console.
- `Modelli`: entità OOP (personaggi, nemici, missioni, oggetti, abilità, enum).
- `Sistemi`: combattimento PvE/PvP, checkpoint, negozio.
- `Servizi`: loader, validator, runner e salvataggio.
- `Dati`: contenuti esterni JSON. Un'aggiunta valida appare al prossimo avvio.

## Dati inclusi

- 12 missioni (3 principali, 1 introduttiva, 8 secondarie)
- 11 tipi di nemici (incluso il boss)
- 6 NPC e 24 linee di dialogo
- 11 oggetti, un negozio configurabile e inventario iniziale esterno
- 6 abilità (2 per classe)

## Stato e persistibilità

Il salvataggio è manuale nel villaggio (opzione 8), mentre il caricamento è nel menu iniziale (opzione 2). Il percorso, avviando il programma dalla radice del progetto, è `Salvataggi/partita.json`.

**Non modificare le definizioni nel salvataggio.** `Dati/*.json` descrive il mondo; `Salvataggi/partita.json` descrive il progresso del giocatore. I checkpoint del combattimento sono solo in memoria. La modalità PvP non cambia la partita PvE.

Il file JSON è aggiornato con scrittura su `.tmp` e sostituzione del file precedente. Non è presente un sistema di backup storico o sincronizzazione cloud.

## Limiti intenzionali

- Caricamento contenuti **all'avvio**, non hot-reload.
- L'aggiunta di nemici comuni, missioni, dialoghi, equipaggiamenti e articoli del negozio è data-driven. Una nuova categoria di effetti o una nuova IA richiede codice C#.
- Solo due abilità per classe nel modello didattico corrente.
- La condotta speciale di `Boss` è la stessa per i boss contrassegnati `Boss: true`.
- Il salvataggio potrebbe rifiutare ID già presenti nella partita se rimossi dal catalogo; non vengono migrate automaticamente le versioni dei salvataggi.
- Le azioni nei menu, il bilanciamento e la sicurezza di tutte le modifiche ai JSON richiedono collaudo effettivo con .NET.

## Stato delle verifiche

Sono stati eseguiti controlli statici di delimitatori, integrità dei file JSON, unicità degli ID, riferimenti e prerequisiti. **Non è stata eseguita una build `.NET`** perché l'SDK non è disponibile nell'ambiente di preparazione. Prima dell'aula eseguire i comandi sopra e seguire `CHECKLIST_TEST_MANUALI.md`.
