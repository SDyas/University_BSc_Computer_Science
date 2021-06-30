package sam.gamecw;

import android.graphics.Canvas;
import android.graphics.Paint;

import java.util.Timer;
import java.util.TimerTask;

public class Obstacle {

    private int x1; // left
    private int y1; // top
    private int x2; // right
    private int y2; // bottom
    private int direction; // 0 for left, 1 for right
    private Paint colour; // colour of obstacle

    public Obstacle(int x1, int y1, int x2, int y2, int direction, Paint colour){

        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
        this.direction = direction;
        this.colour = colour;

        Timer timer = new Timer();
        TimerTask task = new Clock();
        timer.schedule(task, 0, 2000);

    }

    class Clock extends TimerTask {
        public void run() {
            if (direction == 0) direction = 1;
            else if (direction == 1) direction = 0;
        }
    }

    /** draw obstacle rectangle on canvas
     *
     * @param canvas canvas to draw on
     */
    protected void draw(Canvas canvas){
        canvas.drawRect(x1,y1,x2,y2, colour);
    }

    /** set new y positions when moving obstacle back to top
     *
     */
    protected void resetYPositions(){
        y1 = -180;
        y2 = 0;
    }

    /** reset x values
     *
     * @param a new value for x1
     * @param b new value for x2
     */
    protected void resetXPositions(int a, int b){
        x1 = a;
        x2 = b;
    }

    /**
     *  change the y values by 1 to move vertically
     */
    protected void moveVertically(){
        y1 = y1 + 6;
        y2 = y2 + 6;
    }

    /** basic getter to determine obstacle direction
     *
     * @return direction
     */
    protected int getDirection() { return direction; }

    /**
     * change x value by -3 to move left
     */
    protected void moveLeft() {
        if (direction == 0) {
            x1 = x1 - 3;
            x2 = x2 - 3;
        }
    }

    /**
     * change x values by +3 to move right
     */
    protected void moveRight (){
        if (direction == 1) {
            x1 = x1 + 3;
            x2 = x2 + 3;
        }
    }

    /** check if the obstacle is out of bounds
     *
     * @return true if out of bounds
     */
    protected boolean outOfBounds(){
        return y1 > 1920;
    }

    /** check if the obstacle is entering the screen
     *
     * @return true if entering the screen
     */
    protected  boolean enterScreen() { return y2 == 0; }

    /** basic getter
     *
     * @return x1
     */
    protected int getX1(){
        return x1;
    }

    /** basic getter
     *
     * @return x2
     */
    protected int getX2(){
        return x2;
    }

    /** basic getter
     *
     * @return y1
     */
    protected int getY1(){
        return y1;
    }

    /** basic getter
     *
     * @return y2
     */
    protected int getY2(){
        return y2;
    }

    /** basic setter
     *
     * @param x new value for x1
     */
    protected void setX1(int x){
        x1 = x;
    }

    /** basic setter
     *
     * @param x new value for x2
     */
    protected void setX2(int x){
        x2 = x;
    }

    /** basic setter
     *
     * @param y new value for y1
     */
    protected void setY1(int y){
        y1 = y;
    }

    /** basic setter
     *
     * @param y new value for y2
     */
    protected void setY2(int y){
        y2 = y;
    }
}
