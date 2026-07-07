namespace T2SLogistics.Controls;

/// <summary>
/// Entry cujo foco NÃO abre o teclado virtual (Android): o input vem do scanner / keyboard-wedge.
/// Usado no modal de leitura da Expedição (num PDA lê-se por hardware, o teclado só atrapalha).
/// A supressão é aplicada no <c>CustomEntryHandler</c> (Platforms/Android), que só a liga para este tipo.
/// </summary>
public class ScanEntry : Entry
{
}
