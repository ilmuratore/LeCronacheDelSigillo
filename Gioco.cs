using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

class Gioco
{
    public void Avvia()
    {
        bool inEsecuzione = true;

        while (inEsecuzione)
        {
            MostraMenu();

            switch (Console.ReadLine())
            {
                case "1":
                    IniziaPartita();
                    break;
                case "2":
                    MostraIstruzioni();
                    break;
                case "0":
                    inEsecuzione = false;
                    Console.WriteLine("Chiusura del gioco.");
                    break;
                default:
                    Console.WriteLine("Scelta non valida. Riprova.\n");
                    break;
            }
        }

    }
    private void MostraMenu()
    {
        Console.WriteLine("=== LE CRONACHE DEL SIGILLO === ");
        Console.WriteLine("1. Inizia partita");
        Console.WriteLine("2. Istruzioni");
        Console.WriteLine("0. Esci");
        Console.WriteLine("Scelta: ");
    }
    private void IniziaPartita()
    {
        Console.WriteLine("\n Un antico sigillo é stato spezzato...");
        Console.WriteLine("Il viaggio di un eroe che cambierá le sorti del mondo intero ha inizio");
        Console.WriteLine("Sei pronto all'avventura ? ");
        Console.WriteLine("Prima di partire, crea il tuo eroe.");

        string nome = LeggiNome();
        TipoClasse tipoClasse = ScegliClasse();
        Giocatore giocatore = new Giocatore(nome, tipoClasse);
        DistribuisciPunti(giocatore);
        Console.WriteLine("\n === GIOCATORE CREATO ===");
        giocatore.MostraScheda();

        Console.WriteLine("\n Il viaggio nel regno inizia da un piccolo villaggio ...");

        EntraVillaggio(giocatore);

        Console.WriteLine("\n Partita terminata. Premi INVIO per tornare al menu principale");

        Console.ReadLine();
        Console.WriteLine();


    }
    private void MostraIstruzioni()
    {
        Console.WriteLine("\n Digita il numero dell'opzione e premi INVIO.");
        Console.WriteLine("Premi INVIO per tornare al menu.");
        Console.ReadLine();
        Console.WriteLine();
    }
    private string LeggiNome()
    {
        string nome = "";
        while (nome == "")
        {
            Console.WriteLine("Nome del personaggio: ");
            nome = "" + Console.ReadLine();
            nome = nome.Trim();

            if (nome == "")
            {
                Console.WriteLine("Il nome non puo essere vuoto.");
            }
        }

        return nome;
    }
    private TipoClasse ScegliClasse()
    {
        while (true)
        {
            Console.WriteLine(" === SCEGLI LA CLASSE GIOCATORE ===");
            Console.WriteLine("1. Guerriero");
            Console.WriteLine("2. Mago");
            Console.WriteLine("3. Ladro");
            Console.WriteLine("Scelta:  ");

            switch (Console.ReadLine())
            {
                case "1":
                    return TipoClasse.Guerriero;
                case "2":
                    return TipoClasse.Mago;
                case "3":
                    return TipoClasse.Ladro;
                default:
                    Console.WriteLine("Classe non valida. Riprova");
                    break;
            }
        }
    }
    private void DistribuisciPunti(Giocatore giocatore)
    {
        Console.WriteLine($"\n Hai {giocatore.GetPuntiDisponibili()} da distribuire. ");

        while (giocatore.HaPuntiDaDistribuire())
        {
            giocatore.MostraScheda();
            Console.WriteLine(" === Quale statistica vuoi aumentare ? ===");
            Console.WriteLine("1. HP massimi +10");
            Console.WriteLine("2. MP massimi +5");
            Console.WriteLine("3. Attacco +1");
            Console.WriteLine("4. Difesa +1");
            Console.WriteLine("5. Magia +1");
            Console.WriteLine("6. Agilita +1");
            Console.WriteLine("Scelta : ");

            bool assegnato = false;

            switch (Console.ReadLine())
            {
                case "1":
                    assegnato = giocatore.AssegnaPunti(1);
                    break;
                case "2":
                    assegnato = giocatore.AssegnaPunti(2);
                    break;
                case "3":
                    assegnato = giocatore.AssegnaPunti(3);
                    break;
                case "4":
                    assegnato = giocatore.AssegnaPunti(4);
                    break;
                case "5":
                    assegnato = giocatore.AssegnaPunti(5);
                    break;
                case "6":
                    assegnato = giocatore.AssegnaPunti(6);
                    break;
            }
            if (!assegnato)
            {
                Console.WriteLine("Scelta non valida. Nessun punto distribuito.");
            }
        }
    }
    private void EntraVillaggio(Giocatore giocatore)
    {
        Console.WriteLine("\n Sei arrivato in un villaggio, che si trova ai piedi di antiche rovine");
        bool nelVillaggio = true;
        Negozio negozio = new Negozio();
        Missione incarico = new Missione(
            "La prima pattuglia",
            "Il capitano Aldren chiede di fermare un Goblin nei pressi di Valdoria",
            "Goblin Esploratore", 1, 20, 20);
        NPC capitano = new NPC("Capitano Aldren", incarico);
        Missione foresta = new Missione(
            "Il primo sigillo : Foresta",
            "La Saggia Elara ti chiede di fermare il predone che custodisce il primo frammento del sigillo",
            "Goblin Predone", 1, 25, 30);
        Missione rovine = new Missione(
            "Le rovine nel Deserto",
            "Gli indizzi sulla posizione del secondo frammento ti portano alle rovine nel Deserto. Le rovine sono protette da un antico Guardiano",
            "Guardiano delle Rovine", 1, 35, 50);
        Missione custode = new Missione(
            "Il Custode Corrotto",
            "Hai trovato anche il terzo frammento del Sigillo. Uno strano portale si apre dinanzi a te. Un nuovo nemico fa la sua comparsa",
            "Custode Corrotto", 1, 75, 100);
        NPC elara = new NPC("Saggia Elara", foresta);
        Missione lupo = new Missione(
            "Le erbe di Mira",
            "Mira non puo raccogliere le erbe: un lupo minaccia la brughiera", "Lupo della brughiera", 1, 30, 30);
        NPC mira = new NPC("Eroborista Mira", lupo);
        Missione brigante = new Missione(
            "Sulla strada per Valdoria",
            "Il fabbro Doran ha avvistato alcuni briganti lungo la strada per Valdoria, " +
            "libera il passo per ripristinare i suoi affari", "Brigante della strada", 1, 30, 30);
        NPC doran = new NPC("Fabbro Doran", brigante);

        while (nelVillaggio && giocatore.EInVita())
        {
            MostraMenuVillaggio();
            switch (Console.ReadLine())
            {
                case "1":
                    giocatore.MostraScheda();
                    break;
                case "2":
                    bool vittoria = AffrontaGoblin(giocatore, incarico);
                    if (!vittoria)
                    {
                        nelVillaggio = false;
                    }
                    break;
                case "3":
                    negozio.Apri(giocatore);
                    break;
                case "4":
                    ApriBacheca(giocatore, capitano, incarico);
                    break;
                case "5":
                    giocatore.MostraInventario();
                    giocatore.MostraEquipaggiamento();
                    break;
                case "6":
                    EquipaggiaDalVillaggio(giocatore);
                    break;
                case "7":
                    if (!ApriStoria(giocatore, foresta, rovine, custode, elara))
                    {
                        nelVillaggio = false;
                    }
                    break;
                case "8":
                    ApriMissioniSecondarie(giocatore, mira, lupo, doran, brigante);
                    break;
                case "0":
                    nelVillaggio = false;
                    Console.WriteLine("Lasci il villaggio.");
                    break;
                default:
                    Console.WriteLine("Scelta non valida");
                    break;
            }
        }
    }
    private void MostraMenuVillaggio()
    {
        Console.WriteLine("\n === VILLAGGIO DI VALDORIA === ");
        Console.WriteLine("1. Mostra la scheda Personaggio");
        Console.WriteLine("2. Affronta il Goblin (allenamento )");
        Console.WriteLine("3. Negozio");
        Console.WriteLine("4. Bacheca Missioni e NPC");
        Console.WriteLine("5. Inventario e Equipaggiamento");
        Console.WriteLine("6. Equipaggia un oggetto");
        Console.WriteLine("7. Storia Principale: L'Antico Sigillo");
        Console.WriteLine("8. Missioni secondarie");
        Console.WriteLine("0. Termina la partita");
        Console.WriteLine("Scelta: ");
    }
    private bool AffrontaGoblin(Giocatore giocatore, Missione incarico)
    {
        string nomeGoblin = "Goblin Esploratore";
        Nemico goblin = new Nemico(nomeGoblin, 60, 10, 9, 3, 1, 1, 1);
        bool vittoria = AvviaCombattimentoConCheckpoint(giocatore, goblin);

        if (vittoria)
        {
            incarico.RegistraVittoria(nomeGoblin);
            Console.WriteLine("Hai protetto il villaggio di Valdoria");
            Console.WriteLine("Puoi spendere le tue ricompense nel negozio del villaggio");
            Console.WriteLine("I tuoi HP non vengono rigenerati");
        }
        else
        {
            Console.WriteLine("Sei stato sconfitto. La partita termina qui.");
        }
        return vittoria;

    }
    private void EquipaggiaDalVillaggio(Giocatore giocatore)
    {
        giocatore.MostraInventario();
        giocatore.MostraEquipaggiamento();
        Console.WriteLine("0. Annulla");
        Console.WriteLine("Numero dell'oggetto da equipaggiare: ");

        string? risposta = Console.ReadLine();
        if (!int.TryParse(risposta, out int numero))
        {
            Console.WriteLine("Devi inserire un numero.");
            return;
        }
        if (numero == 0)
        {
            Console.WriteLine("Nessuna modifica.");
            return;
        }
        if (giocatore.Equipaggia(numero))
        {
            giocatore.MostraEquipaggiamento();
            Console.WriteLine("Statistiche aggiornate");
            giocatore.MostraScheda();
        }
    }
    private void ApriBacheca(Giocatore giocatore, NPC capitano, Missione incarico)
    {
        bool nellaBacheca = true;

        while (nellaBacheca)
        {
            Console.WriteLine("\n === BACHECA MISSIONI VALDORIA ===");
            Console.WriteLine("1. Parla con il Capitano Aldren");
            Console.WriteLine("2. Consulta l'incarico");
            Console.WriteLine("0. Torna al villaggio");
            Console.WriteLine("Scelta: ");

            switch (Console.ReadLine())
            {
                case "1":
                    capitano.Parla(giocatore);
                    break;
                case "2":
                    incarico.Mostra();
                    break;
                case "0":
                    nellaBacheca = false;
                    break;
                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
                    ; }


        }
    }
    private bool ApriStoria(Giocatore giocatore, Missione foresta, Missione rovine, Missione custode, NPC elara)
    {
        bool nelMenuStoria = true;
        while(nelMenuStoria && giocatore.EInVita())
        {
            Console.WriteLine("\n === STORIA PRINCIPALE : L'ANTICO SIGILLO ===");
            Console.WriteLine("1. Parla con la Saggia Elara");
            Console.WriteLine("2. Parti per la missione principale");
            Console.WriteLine("3. Raggiungi il Portale del Custode");
            Console.WriteLine("4. Consulta il Diario");
            Console.WriteLine("0. Torna al villaggio");
            Console.WriteLine("Scelta: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ParlaConElara(giocatore, foresta, rovine, custode, elara);
                    break;
                case "2":
                    if (foresta.LeggiStato() == StatoMissione.InCorso && !foresta.EProntaPerConsegna())
                    {
                        Console.WriteLine("\n Entri nella foresta dei Goblin.");
                        AffrontaNemicoMissione(giocatore, foresta, new Nemico("Goblin Predone", 60, 30, 10, 5, 5, 10, 5));
                    } else if (foresta.LeggiStato() == StatoMissione.Completata &&
                        rovine.LeggiStato() == StatoMissione.InCorso &&
                        !rovine.EProntaPerConsegna())
                    {
                        Console.WriteLine("\n Esplori le Rovine nel Deserto. Ti senti osservato.");
                        AffrontaNemicoMissione(giocatore, rovine, new Nemico("Guardiano delle Rovine", 100, 10, 10, 10, 5, 5, 5));
                    }
                    else
                    {
                        Console.WriteLine("Parla con Elara per riunire i frammenti del Sigillo");
                    }
                    break;
                case "3":
                    if(rovine.LeggiStato() != StatoMissione.Completata)
                    {
                        Console.WriteLine("Il portale é sigillato: servono entrambi i frammenti del Sigillo");
                    } 
                    else if(custode.LeggiStato() == StatoMissione.NonIniziata)
                    {
                        Console.WriteLine("Il portale é aperto. Parla con Elara.");
                    } 
                    else if(custode.LeggiStato() == StatoMissione.Completata)
                    {
                        Console.WriteLine("Il Sigillo é stato aperto. Hai salvato Valdoria");
                    }
                    else if (custode.EProntaPerConsegna())
                    {
                        Console.WriteLine("Hai sconfitto il Custode. Torna da Elara per concludere la Storia Principale");
                    }
                    else
                    {
                        Console.WriteLine("Il Custode Corrotto ti attende oltre il portale.");
                        AffrontaNemicoMissione(giocatore, custode, new Boss());
                    }
                    break;
                case "4":
                    MostraDiarioStoria(foresta, rovine, custode);
                    break;
                case "0":
                    nelMenuStoria = false;
                    break;
                default:
                    break;
            }
        }
        return giocatore.EInVita();

    }
    private void MostraDiarioStoria(Missione foresta, Missione rovine, Missione custode)
    {
        Console.WriteLine("\n === DIARIO STORIA ===");
        foresta.Mostra();
        if (foresta.LeggiStato() == StatoMissione.Completata)
        {
            rovine.Mostra();
        }
        else
        {
            Console.WriteLine("\n Fase 2: Rovine nel Deserto (bloccata)");
        }

        if (rovine.LeggiStato() == StatoMissione.Completata)
        {
            custode.Mostra();
        }
        else
        {
            Console.WriteLine("\n Fase 3: Custode Corrotto (bloccata)");
        }
    }
    private void ParlaConElara(Giocatore giocatore, Missione foresta, Missione rovine, Missione custode, NPC elara)
    {
        if (foresta.LeggiStato() != StatoMissione.Completata)
        {
            elara.Parla(giocatore, foresta);
        } else if (rovine.LeggiStato() != StatoMissione.Completata)
        {
            Console.WriteLine("Elara: Ottimo hai recuperato il primo frammento e sconfitto il predone. Ora dirigiti verso le Rovine nel Deserto. Fai attenzione un antico e potente Guardiano difende il secondo frammento.");
            elara.Parla(giocatore, rovine);
        }
        else
        {
            Console.WriteLine("Elara: Hai riunito tutti i frammenti. Ma cosa sta succedendo. Guarda un portale dimensionale! Si percepisce un potere enorme dall'altro lato");
            bool finaleDaConsegnare = custode.EProntaPerConsegna();
            elara.Parla(giocatore, custode);
            if(finaleDaConsegnare && custode.LeggiStato() == StatoMissione.Completata)
            {
                Console.WriteLine("\n === FINE DELLA STORIA === ");
                Console.WriteLine("Il Custode Corrotto é stato sconfitto");
                Console.WriteLine("La chiave apre il Sigillo.");
                Console.WriteLine("Valdoria é salva. Grazie eroe");
            }
        }
    }
    private bool AffrontaNemicoMissione(Giocatore giocatore, Missione missione, Nemico nemico)
    {
        Checkpoint checkpoint = new Checkpoint(giocatore);
        checkpoint.MostraConferma();
        bool vittoria = AvviaCombattimentoConCheckpoint(giocatore, nemico);
        if (!vittoria)
        {
            Console.WriteLine("Sei stato sconfitto. La partita termina qui.");
            return false;
        }
        missione.RegistraVittoria(nemico.LeggiNome());
        Console.WriteLine("Obiettivo raggiunto! Riporta lo stato della missione per completarla.");
        return true;
    } 
    private void ApriMissioniSecondarie(Giocatore giocatore, NPC mira, Missione lupo, NPC doran, Missione brigante)
    {
        bool nelMenu = true;
        while (nelMenu && giocatore.EInVita())
        {
            Console.WriteLine("\n === MISSIONI SECONDARIE === ");
            Console.WriteLine("1. Parla con l'Erborista Mira");
            Console.WriteLine("2. Parla con il Fabbro Doran");
            Console.WriteLine("3. Consulta gli incarichi");
            Console.WriteLine("4. Esplora la Brughiera (Lupo)");
            Console.WriteLine("5. Percorri la strada (Briganti)");
            Console.WriteLine("0. Torna al villaggio");
            Console.Write("Scelta: ");

            switch (Console.ReadLine())
            {
                case "1":
                    mira.Parla(giocatore);
                    break;
                case "2":
                    doran.Parla(giocatore);
                    break;
                case "3":
                    lupo.Mostra();
                    brigante.Mostra();
                    break;
                case "4":
                    AffrontaMissioneSecondaria(giocatore, lupo, new Nemico("Lupo della Brughiera", 50, 0, 15, 10, 3, 0, 15));
                    break;
                case "5":
                    AffrontaMissioneSecondaria(giocatore, brigante, new Nemico("Brigante della strada", 60, 30, 20, 10, 10, 20, 25));
                    break;
                case "0":
                    nelMenu = false;
                    break;
                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }

        }
    }
    private void AffrontaMissioneSecondaria(Giocatore giocatore, Missione missione, Nemico nemico)
    {
        if(missione.LeggiStato() == StatoMissione.NonIniziata)
        {
            Console.WriteLine("Parla prima con un abitante del villaggio per accettare la missione");
            return;
        }
        if(missione.LeggiStato() == StatoMissione.Completata)
        {
            Console.WriteLine("Hai giá completato questa missione");
            return;
        }
        if (missione.EProntaPerConsegna())
        {
            Console.WriteLine("Parla con l'abitante per riscuotere la tua ricompensa");
            return;
        }
        AffrontaNemicoMissione(giocatore, missione, nemico);
    }
    private bool AvviaCombattimentoConCheckpoint(Giocatore giocatore, Nemico nemico)
    {
        Checkpoint checkpoint = new Checkpoint(giocatore);
        checkpoint.MostraConferma();

        while (true)
        {
            Combattimento combattimento = new Combattimento(giocatore, nemico);
            if (combattimento.Inizia())
            {
                return true;
            }
            while (true)
            {
                Console.WriteLine("Vuoi riprovare questo combattimento ? ");
                Console.WriteLine("1. Riprova dal checkpoint");
                Console.WriteLine("0. Termina la partita");
                Console.WriteLine("Scelta: ");
                string? scelta = Console.ReadLine();
                if(scelta == "1")
                {
                    checkpoint.Ripristina(giocatore);
                    nemico.PreparaRivincita();
                    Console.WriteLine("Checkpoint ricaricato.");
                    break;
                }
                if(scelta == "0" || scelta == null)
                {
                    return false;
                }
                Console.WriteLine("Scelta non valida.");
            }
        }
    }
}

