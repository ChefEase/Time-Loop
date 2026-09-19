// Stable knowledge IDs, not world facts or automatic discovery triggers.
// Story examples reserve identifiers; they do not settle open canon questions.
public static class KnowledgeFacts
{
    public const string SecretKnown = "test.secret_known";
    public const string DanielStoleKey = "event.daniel_stole_key";
    public const string TheoWitnessedTheft = "event.theo_witnessed_theft";
    public const string AdrianHiredDaniel = "event.adrian_hired_daniel";
    public const string JonahCarriesRelay = "event.jonah_carries_relay";
    public const string RelayNeededByMara = "fact.relay_needed_by_mara";
    public const string AdrianKilledMercer = "event.adrian_killed_mercer";
    public const string IrisSawAdrian = "event.iris_saw_adrian";
    public const string FelixPhotoShowsAdrian = "evidence.felix_photo_adrian";
    public const string ChurchTunnelExists = "location.church_tunnel";
    public const string MeridianExists = "history.meridian_exists";
    public const string LydiaWasArthursDaughter = "history.lydia_wren";
    public const string PhaseEchoesAreRecordings = "history.phase_echo_truth";

    // Three-minute prototype observations. These persist across loop reloads.
    public const string PrototypeTheoReportedDaniel = "prototype.theo_reported_daniel";
    public const string PrototypeReportCausesChase = "prototype.report_causes_chase";

    // Notebook MVP discovery and display facts.
    public const string PersonDaniel = "person.daniel_cross";
    public const string PersonTheo = "person.theo_shaw";
    public const string PersonRuth = "person.ruth_reed";
    public const string TimelineDanielStoleKey = DanielStoleKey;
    public const string TimelineTheoReportedDaniel = PrototypeTheoReportedDaniel;
    public const string TimelineJonahInjured = "prototype.jonah_injured";
    public const string ClueBrassKey = "clue.brass_key_discovered";
    public const string ClueDeliveryParcel = "clue.delivery_parcel_discovered";
    public const string ClueWitnessStatement = "clue.witness_statement_discovered";
}
