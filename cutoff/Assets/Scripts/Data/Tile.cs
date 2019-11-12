
public class Tile {
    public int x;
    public int y;

    public int TileOwner;

    public Tile(int x, int y, int tileOwner = 0) {
        this.x = x;
        this.y = y;
        this.TileOwner = tileOwner;
    }
}
