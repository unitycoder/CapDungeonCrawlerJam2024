using CaptainCoder.Dungeoneering.DungeonCrawler;
using CaptainCoder.Dungeoneering.DungeonMap;
using CaptainCoder.Dungeoneering.Player;

namespace CaptainCoder.Dungeoneering.Game;

public class CrawlerMode(DungeonCrawlerManifest manifest, Dungeon currentDungeon, PlayerView playerView)
{
    public DungeonCrawlerManifest Manifest { get; set; } = manifest;
    private Dungeon _currentDungeon = currentDungeon;
    public Dungeon CurrentDungeon
    {
        get => _currentDungeon;
        set
        {
            if (_currentDungeon == value) { return; }
            var exited = _currentDungeon;
            _currentDungeon = value;
            OnDungeonChange?.Invoke(new DungeonChangeEvent(exited, _currentDungeon));
        }
    }
    private PlayerView _currentView = playerView;
    public PlayerView CurrentView
    {
        get => _currentView;
        set
        {
            if (_currentView == value) { return; }
            Position prevPosition = _currentView.Position;
            PlayerView previous = _currentView;
            _currentView = value;
            OnViewChange?.Invoke(new ViewChangeEvent(previous, _currentView));
            if (prevPosition != _currentView.Position)
            {
                OnPositionChange?.Invoke(new PositionChangeEvent(prevPosition, _currentView.Position));
            }
        }
    }
    public event Action<DungeonChangeEvent>? OnDungeonChange;
    public event Action<ViewChangeEvent>? OnViewChange;
    public event Action<PositionChangeEvent>? OnPositionChange;
    public event Action<Message>? OnMessageAdded;
    public void AddMessage(Message message) => OnMessageAdded?.Invoke(message);
    public void AddMessage(string message) => OnMessageAdded?.Invoke(new Message(message));

}

public record DungeonChangeEvent(Dungeon Exited, Dungeon Entered);
public record PositionChangeEvent(Position Exited, Position Entered);
public record ViewChangeEvent(PlayerView Exited, PlayerView Entered);

public record Message(MessageType MessageType, string Text)
{
    public Message(string text) : this(MessageType.Info, text) { }
}

public enum MessageType
{
    Debug,
    Info,
}