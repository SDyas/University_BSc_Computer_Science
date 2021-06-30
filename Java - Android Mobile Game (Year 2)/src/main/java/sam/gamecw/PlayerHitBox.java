package sam.gamecw;

import android.graphics.Paint;

public class PlayerHitBox extends Player {

    public PlayerHitBox(int x1, int y1, int x2, int y2, int direction, Paint colour) {
        super(x1, y1, x2, y2, direction, colour);
    }

    /** set the position to follow the player
     *
     * @param inputX player x position
     * @param inputY player y position
     */
    protected void setPosition(int inputX, int inputY){
        x1 = inputX - 30;
        x2 = inputX + 30;
        y1 = inputY - 30;
        y2 = inputY + 30;
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
     * @return y2
     */
    protected int getY2(){
        return y2;
    }

}
