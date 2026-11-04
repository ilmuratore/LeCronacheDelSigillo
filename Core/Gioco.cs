using System;
using System.Collections.Generic;

// La logica e unica; missioni, nemici, dialoghi, negozio e abilita arrivano dal Catalogo.
class Gioco
{
    private Catalogo catalogo;
    public Gioco(Catalogo catalogo) { this.catalogo = catalogo; }

    public void Avvia()
    {
        while (true)
        {
            Schermata.Titolo("LE CRONACHE DEL SIGILLO - V2");
            Console.WriteLine("1. Nuova partita PvE");
            Console.WriteLine("2. Carica partita JSON");
            Console.WriteLine("3. Arena PvP locale");
            Console.WriteLine("4. Istruzioni");
            Console.WriteLine("0. Esci");
            Console.Write("Scelta: ");
            string? scelta = Console.ReadLine();
            if (scelta == null || scelta == "0") return;
            if (scelta == "1") NuovaPartita();
            else if (scelta == "2")
            {
                Partita? caricata = ServizioSalvataggi.Carica(catalogo);
                Schermata.Pausa();
                if (caricata != null) Villaggio(caricata);
            }
            else if (scelta == "3") AvviaPvP();
            else if (scelta == "4")
            {
                Console.WriteLine("PvE: missioni, negozio, equipaggiamento, potenziamenti e salvataggi.");
                Console.WriteLine("PvP: due giocatori, stessa tastiera; nessun salvataggio.");
                Console.WriteLine("I file nella cartella Dati possono essere modificati e ricaricati.");
                Schermata.Pausa();
            }
        }
    }

    private void NuovaPartita()
    {
        Giocatore giocatore = CreaEroe("CREAZIONE EROE PVE");
        Villaggio(new Partita(giocatore, catalogo));
    }

    private Giocatore CreaEroe(string titolo)
    {
        Schermata.Titolo(titolo);
        Console.Write("Nome: ");
        string nome = (Console.ReadLine() ?? "").Trim();
        if (nome == "") nome = "Avventuriero";
        TipoClasse classe;
        while (true)
        {
            Console.WriteLine("1. Guerriero  2. Mago  3. Ladro");
            Console.Write("Classe: ");
            string? risposta = Console.ReadLine();
            if (risposta == "1") { classe = TipoClasse.Guerriero; break; }
            if (risposta == "2") { classe = TipoClasse.Mago; break; }
            if (risposta == "3") { classe = TipoClasse.Ladro; break; }
            Console.WriteLine("Classe non valida.");
        }
        Giocatore giocatore = new Giocatore(nome, classe);
        giocatore.CaricaAbilita(catalogo.Abilita);
        giocatore.CaricaInventarioIniziale(catalogo);
        while (giocatore.HaPuntiDaDistribuire())
        {
            giocatore.MostraScheda();
            Console.WriteLine("Assegna 1 punto: 1 HP(+10), 2 MP(+5), 3 Attacco, " +
                              "4 Difesa, 5 Magia, 6 Agilita");
            Console.Write("Scelta: ");
            if (!int.TryParse(Console.ReadLine(), out int scelta) ||
                !giocatore.AssegnaPunto(scelta))
                Console.WriteLine("Scelta non valida.");
        }
        return giocatore;
    }

    private void AvviaPvP()
    {
        Giocatore primo = CreaEroe("ARENA PVP: GIOCATORE 1");
        Giocatore secondo = CreaEroe("ARENA PVP: GIOCATORE 2");
        new CombattimentoPvP(primo, secondo).Inizia();
    }

    private void Villaggio(Partita partita)
    {
        Negozio negozio = new Negozio(catalogo, catalogo.Negozi[0].Id);
        while (partita.Eroe.EInVita())
        {
            Schermata.Titolo("VILLAGGIO DI VALDORIA");
            Console.WriteLine("Eroe: " + partita.Eroe.LeggiNome());
            Console.WriteLine("1. Scheda personaggio");
            Console.WriteLine("2. Bacheca missioni e dialoghi NPC");
            Console.WriteLine("3. Spedizioni e combattimenti");
            Console.WriteLine("4. Negozio");
            Console.WriteLine("5. Inventario ed equipaggiamento");
            Console.WriteLine("6. Equipaggia oggetto");
            Console.WriteLine("7. Punti abilita e potenziamenti");
            Console.WriteLine("8. Salva partita JSON");
            Console.WriteLine("0. Torna al menu (la partita non viene salvata automaticamente)");
            Console.Write("Scelta: ");
            string? scelta = Console.ReadLine();
            if (scelta == "0" || scelta == null) return;
            if (scelta == "1") partita.Eroe.MostraScheda();
            else if (scelta == "2") Bacheca(partita);
            else if (scelta == "3")
            {
                if (!Spedizioni(partita)) return;
            }
            else if (scelta == "4") negozio.Apri(partita.Eroe);
            else if (scelta == "5")
            {
                partita.Eroe.MostraInventario();
                partita.Eroe.MostraEquipaggiamento();
            }
            else if (scelta == "6")
            {
                partita.Eroe.MostraInventario();
                Console.Write("Numero dell'oggetto da equipaggiare (0 indietro): ");
                if (int.TryParse(Console.ReadLine(), out int n) && n != 0)
                    partita.Eroe.Equipaggia(n);
            }
            else if (scelta == "7") Potenziamenti(partita.Eroe);
            else if (scelta == "8") ServizioSalvataggi.Salva(partita);
            else Console.WriteLine("Scelta non valida.");
            Schermata.Pausa();
        }
    }

    private void Bacheca(Partita partita)
    {
        Schermata.Titolo("BACHECA - MISSIONI DAL CATALOGO JSON");
        for (int i = 0; i < partita.Missioni.Count; i++)
        {
            Missione m = partita.Missioni[i];
            Console.Write((i + 1) + ". " + m.LeggiTitolo() + " [" + m.LeggiCategoria() + "]");
            if (!m.Sbloccata(partita.Missioni)) Console.Write(" - BLOCCATA");
            else Console.Write(" - " + m.LeggiStato());
            Console.WriteLine();
        }
        Console.WriteLine("0. Indietro");
        Console.Write("Missione: ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n == 0 || n > partita.Missioni.Count || n < 0)
            return;
        Missione missione = partita.Missioni[n - 1];
        if (!missione.Sbloccata(partita.Missioni))
        {
            Console.WriteLine("Completa prima la missione richiesta.");
            return;
        }
        NPC npc = new NPC(catalogo.TrovaNPC(missione.LeggiNpcId()));
        npc.Parla(missione, catalogo);
        if (missione.LeggiStato() == StatoMissione.NonIniziata)
        {
            Console.Write("1. Accetta la missione: ");
            if (Console.ReadLine() == "1" && missione.Accetta(partita.Missioni))
                Console.WriteLine("Missione accettata.");
        }
        else if (missione.EProntaPerConsegna())
        {
            Console.Write("1. Consegna la missione: ");
            if (Console.ReadLine() == "1" && missione.Consegna(partita.Eroe))
            {
                Console.WriteLine("Missione consegnata. Premio ottenuto una sola volta.");
                if (catalogo.TrovaMissione(missione.LeggiId()).Finale)
                    Console.WriteLine("=== FINE DELLA STORIA === Il Sigillo torna a splendere!");
            }
        }
        else Console.WriteLine("Nessuna azione disponibile per questa missione.");
    }

    private bool Spedizioni(Partita partita)
    {
        List<Missione> disponibili = new List<Missione>();
        foreach (Missione m in partita.Missioni)
            if (m.LeggiStato() == StatoMissione.InCorso && !m.EProntaPerConsegna())
                disponibili.Add(m);
        Schermata.Titolo("SPEDIZIONI DISPONIBILI");
        if (disponibili.Count == 0)
        {
            Console.WriteLine("Accetta prima una missione dalla bacheca.");
            return true;
        }
        for (int i = 0; i < disponibili.Count; i++)
            Console.WriteLine((i + 1) + ". " + disponibili[i].LeggiTitolo() +
                " (" + disponibili[i].LeggiProgresso() + "/" + disponibili[i].LeggiQuantita() + ")");
        Console.WriteLine("0. Indietro");
        Console.Write("Spedizione: ");
        if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0 || n > disponibili.Count)
            return true;
        Missione missione = disponibili[n - 1];
        Nemico nemico = catalogo.CreaNemico(missione.LeggiNemicoId());
        bool vinto = CombattimentoConRivincita(partita.Eroe, nemico);
        if (vinto) missione.RegistraVittoria(missione.LeggiNemicoId());
        else Console.WriteLine("Tentativo terminato. Torna al menu e ricarica una partita se necessario.");
        return vinto;
    }

    private bool CombattimentoConRivincita(Giocatore giocatore, Nemico nemico)
    {
        Checkpoint checkpoint = new Checkpoint(giocatore);
        while (true)
        {
            Combattimento combattimento = new Combattimento(giocatore, nemico);
            if (combattimento.Inizia()) return true;
            while (true)
            {
                Console.WriteLine("1. Riprova dal checkpoint | 0. Termina partita");
                Console.Write("Scelta: ");
                string? scelta = Console.ReadLine();
                if (scelta == "1")
                {
                    checkpoint.Ripristina(giocatore);
                    nemico.PreparaRivincita();
                    break;
                }
                if (scelta == "0" || scelta == null) return false;
            }
        }
    }

    private void Potenziamenti(Giocatore giocatore)
    {
        while (true)
        {
            Schermata.Titolo("PUNTI ABILITA");
            giocatore.MostraAbilita();
            Console.WriteLine("Ogni livello guadagnato = +1 punto abilita; grado massimo 3.");
            Console.WriteLine("0. Indietro");
            Console.Write("Numero abilita da potenziare: ");
            if (!int.TryParse(Console.ReadLine(), out int n) || n == 0) return;
            if (!giocatore.PotenziaAbilita(n))
                Console.WriteLine("Potenziamento non disponibile (punti, livello o grado massimo).");
            else Console.WriteLine("Potenziamento riuscito. Bonus danno +3 per grado.");
            Schermata.Pausa();
        }
    }
}
