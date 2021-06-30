package sam.gamecw;

import android.graphics.Canvas;
import android.graphics.Paint;

public class Player {

    protected int x1; // left
    protected int y1; // top
    protected int x2; // right
    protected int y2; // bottom
    private int direction; // 0 for left, 1 for right
    private Paint colour; // colour of obstacle


    public Player(int x1, int y1, int x2, int y2, int direction, Paint colour) {

        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.direction = direction;
        this.colour = colour;
    }

    /** draw player circle on canvas
     *
     * @param canvas canvas to draw on
     */
    protected void draw(Canvas canvas){
        canvas.drawCircle(x1,y1,y2,colour);
    }

    /** basic getter
     *
     * @return x1
     */
    protected int getX1(){
        return x1;
    }

    /** basic getter
     *
     * @return y1
     */
    protected int getY1(){
        return y1;
    }

    /** basic getter to determine player direction
     *
     * @return direction
     */
    protected int getDirection() { return direction; }

    /**
     * change the direction int to right if going left vice versa
     */
    protected void changeDirection(){
        if (direction == 0) direction = 1;
        else if (direction == 1) direction = 0;
    }

    /**
     * moves the player left by changing x values
     */
    protected void moveLeft(){
        x1 = x1 - 8;
        x2 = x2 - 8;
    }

    /**
     * moves the player right by changing x values
     */
    protected void moveRight(){
        x1 = x1 + 8;
        x2 = x2 + 8;
    }

    /** checks if player is out of bounds
     *
     */
    protected void playerOutOfBounds() {
        if (x1 < 0) {
            x1 = 1080;
        }
        else if (x1 > 1080){
            x1 = 0;
        }
    }
}
