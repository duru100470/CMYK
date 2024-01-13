public interface IMoveable
{
    void Move(Coordinate dir);
    bool TryMove(Coordinate dir);
}