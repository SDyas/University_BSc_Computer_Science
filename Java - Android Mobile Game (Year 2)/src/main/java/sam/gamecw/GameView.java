package sam.gamecw;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.view.MotionEvent;
import android.view.SurfaceHolder;
import android.view.SurfaceView;
import java.util.ArrayList;
import java.util.Random;

public class GameView extends SurfaceView implements SurfaceHolder.Callback {

    private MainThread thread;

    private ArrayList<Obstacle> obstacleAList = new ArrayList<>(); // arrayList for A obstacles
    private ArrayList<Obstacle> obstacleBList = new ArrayList<>(); // arrayList for B obstacles
    private ArrayList<Player> playerList = new ArrayList<>(); // arrayList to store player
    private ArrayList<PlayerHitBox> playerHitBoxList = new ArrayList<>(); // arrayList to store the player hit box

    private int prevObType; // obstacle type of previous obstacle

    private boolean gameStart = true; // true when game starting
    private boolean gameOn = false; // true when game running
    private boolean gameOver = false; // true when game over
    private boolean gameOverPause = true; // stops onTouchEvent when gameOver first true

    private Paint nextColour = new Paint(); // colour of next obstacles

    private int colourInt = 0;
    private int score = 0;

    private int endWhiteBoxBound1;
    private int endWhiteBoxBound2;

    private Random rand = new Random(); // random number generator

    public GameView(Context context) {
        super(context);
        getHolder().addCallback(this);
        thread = new MainThread(getHolder(), this);
        setFocusable(true);
    }

    @Override
    public void surfaceChanged(SurfaceHolder holder, int format, int width, int height) {
    }

    @Override
    public void surfaceCreated(SurfaceHolder holder) {
        Player player = new Player(540, 960, 0, 30, 0, nextColour);
        playerList.add(player);
        thread.setRunning(true);
        thread.start();
    }

    @Override
    public void surfaceDestroyed(SurfaceHolder holder) {
        boolean retry = true;
        while (retry) {
            try {
                thread.setRunning(false);
                thread.join();
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            retry = false;
        }
    }

    @Override
    public boolean onTouchEvent(MotionEvent e) {
        if (gameStart) {
            initialiseGame();
            gameStart = false;
            gameOn = true;
        } else if (gameOn) {
            for (Player player : playerList) player.changeDirection();
        } else if (gameOver) {
            if (gameOverPause){
                gameOverPause = false;
            }
            else {
                resetGame();
                gameOver = false;
                gameOn = true;
                gameOverPause = true;
            }
        }
        return true;
    }

    /**
     * updates the objects on the canvas:
     * 1. move obstacles vertically
     * 2. switch direction every four seconds
     */
    public void update() {

        if (gameOn) {

            for (Player player : playerList) {
                if (player.getDirection() == 0) player.moveLeft();
                else if (player.getDirection() == 1) player.moveRight();
                player.playerOutOfBounds();
            }

            for (int obCount = 0; obCount < obstacleAList.size(); obCount++) {

                obstacleAList.get(obCount).moveVertically();
                obstacleBList.get(obCount).moveVertically();

                if (score >= 20) {
                    if (obstacleAList.get(obCount).getDirection() == 0) {
                        obstacleAList.get(obCount).moveLeft();
                        obstacleBList.get(obCount).moveLeft();
                    } else if (obstacleAList.get(obCount).getDirection() == 1) {
                        obstacleAList.get(obCount).moveRight();
                        obstacleBList.get(obCount).moveRight();
                    }
                }

                if (obstacleAList.get(obCount).enterScreen()) colourInt++;
                if (colourInt == 7) colourInt = 0;

                if (obstacleAList.get(obCount).getY1() == 960) score++;

                if (obstacleAList.get(obCount).outOfBounds()) {

                    colourInt++;
                    if (colourInt == 7) colourInt = 0;

                    obstacleAList.get(obCount).resetYPositions();
                    obstacleBList.get(obCount).resetYPositions();

                    // obstacle type for obstacle reset
                    int newObType = rand.nextInt(3); // randomise newObType
                    while (newObType == prevObType) {
                        newObType = rand.nextInt(3);
                    }
                    if (newObType == 0) { // mid gap
                        obstacleAList.get(obCount).resetXPositions(-1080, 378); // mid gap
                        obstacleBList.get(obCount).resetXPositions(702, 2160); // mid gap
                        prevObType = 0;
                    } else if (newObType == 1) { // left gap
                        obstacleAList.get(obCount).resetXPositions(-1000, 216); // left gap
                        obstacleBList.get(obCount).resetXPositions(540, 2160); // left gap
                        prevObType = 1;
                    } else if (newObType == 2) { // right gap
                        obstacleAList.get(obCount).resetXPositions(-1000, 540); // right gap
                        obstacleBList.get(obCount).resetXPositions(864, 2160); // right gap
                        prevObType = 2;
                    }
                }
            }
        }
    }

    /**
     * draws the canvas
     *
     * @param canvas canvas to be drawn on
     */
    @Override
    public void draw(Canvas canvas) {
        super.draw(canvas);
        if (canvas != null) {
            canvas.drawColor(Color.WHITE);

            // draw each object

            for (Obstacle obstacle : obstacleAList) obstacle.draw(canvas);
            for (Obstacle obstacle : obstacleBList) obstacle.draw(canvas);
            for (Player player : playerList) player.draw(canvas);

            // set the player hitBox to the location of the player

            for (PlayerHitBox hitBox : playerHitBoxList)
                hitBox.setPosition(playerList.get(0).getX1(), playerList.get(0).getY1());

            if (gameStart){
                nextColour.setTextSize(150);
                canvas.drawText("Colour Dodge", 45, 520, nextColour);
                canvas.drawText("By Sam Dyas" ,45, 1400, nextColour);
                nextColour.setTextSize(100);
                canvas.drawText("Tap to Play", 45, 1600, nextColour);

            }

            if (gameOn) {
                nextColour.setTextSize(75);
                canvas.drawText("" + score, playerList.get(0).x1 + 50, 1000, nextColour);
            }
            changeColour(); // cycle through colours of the rainbow
            checkCollision(); // check for collision between players and obstacles

            // game over screen

            if (gameOver) {
                nextColour.setTextSize(200);

                Paint white = new Paint();
                white.setColor(Color.WHITE);

                canvas.drawRect(0, 0, 1080, endWhiteBoxBound1 -100, white);
                canvas.drawText("Game Over", 45, 520, nextColour);

                canvas.drawRect(0, endWhiteBoxBound2 + 100, 1080, 1920, white);
                canvas.drawText("Score: " + score, 45, 1400, nextColour);

                nextColour.setTextSize(100);
                canvas.drawText("Tap to Play Again", 45, 1600, nextColour);

            }
        }
    }

    /**
     * create two rectangles obstacle and add arrayList
     */
    private void createTwoRectObstacle(int rectAx2, int rectBx1, int y1, int y2) {

        Obstacle obstacleA = new Obstacle(-1080, y1, rectAx2, y2, 0, nextColour);
        obstacleAList.add(obstacleA);
        Obstacle obstacleB = new Obstacle(rectBx1, y1, 2160, y2, 0, nextColour);
        obstacleBList.add(obstacleB);
    }

    /**
     * creates the first four initial obstacles
     */
    private void createInitialObstacles() {

        // middle gap 378,702
        // left gap 216, 540
        // right gap 540, 864

        int y1; // top of obstacle position
        int y2; // bottom of obstacle position
        for (int obCount = 0; obCount < 4; obCount++) {

            if (obCount == 0) { // first obstacle position
                y1 = -180;
                y2 = 0;
            } else if (obCount == 1) { // second obstacle position
                y1 = -720;
                y2 = -540;
            } else if (obCount == 2) { // third obstacle position
                y1 = -1260;
                y2 = -1080;
            } else { // fourth obstacle position;
                y1 = -1800;
                y2 = -1620;
            }

            int type = rand.nextInt(3);
            while (type == prevObType) {
                type = rand.nextInt(3);
            }
            if (type == 0) {
                createTwoRectObstacle(378, 702, y1, y2); // middle gap
                prevObType = 0;
            } else if (type == 1) {
                createTwoRectObstacle(216, 540, y1, y2); // left gap
                prevObType = 1;
            } else if (type == 2) {
                createTwoRectObstacle(540, 864, y1, y2); // right gap
                prevObType = 2;
            }
        }
    }

    /**
     * change the colour of the next obstacle and increment score by 1
     */
    private void changeColour() {
        if (colourInt == 0) {
            nextColour.setColor(getResources().getColor(R.color.MyRed));
        } else if (colourInt == 1) {
            nextColour.setColor(getResources().getColor(R.color.MyOrange));
        } else if (colourInt == 2) {
            nextColour.setColor(getResources().getColor(R.color.MyYellow));
        } else if (colourInt == 3) {
            nextColour.setColor(getResources().getColor(R.color.MyGreen));
        } else if (colourInt == 4) {
            nextColour.setColor(getResources().getColor(R.color.MyBlue));
        } else if (colourInt == 5) {
            nextColour.setColor(getResources().getColor(R.color.MyIndigo));
        } else if (colourInt == 6) {
            nextColour.setColor(getResources().getColor(R.color.MyViolet));
        }
    }

    /**
     * checks for collision between player hitBox and obstacles
     * if collision is detected the game will end
     */
    private void checkCollision() {
        for (Obstacle obstacle : obstacleAList) {
            if (obstacle.getX1() < playerHitBoxList.get(0).getX2() && obstacle.getX2() > playerHitBoxList.get(0).getX1() &&
                    obstacle.getY1() < playerHitBoxList.get(0).getY2() && obstacle.getY2() > playerHitBoxList.get(0).getY1()) {

                endWhiteBoxBound1 = obstacle.getY1();
                endWhiteBoxBound2 = obstacle.getY2();

                gameOn = false;
                gameOver = true;
                break;
            }
        }
        for (Obstacle obstacle : obstacleBList) {
            if (obstacle.getX1() < playerHitBoxList.get(0).getX2() && obstacle.getX2() > playerHitBoxList.get(0).getX1() &&
                    obstacle.getY1() < playerHitBoxList.get(0).getY2() && obstacle.getY2() > playerHitBoxList.get(0).getY1()) {

                endWhiteBoxBound1 = obstacle.getY1();
                endWhiteBoxBound2 = obstacle.getY2();

                gameOn = false;
                gameOver = true;
                break;
            }
        }
    }

    /**
     * create game objects
     */
    private void initialiseGame() {
        createInitialObstacles();
        PlayerHitBox hitBox = new PlayerHitBox(0, 0, 0, 0, 0, nextColour);
        playerHitBoxList.add(hitBox);
    }

    /**
     * reset obstacle and player positions, reset score
     */
    private void resetGame() {

        // move objects

        for (int obCount = 0; obCount < 4; obCount++) {

            if (obCount == 0) { // first obstacle position
                obstacleAList.get(obCount).setY1(-180);
                obstacleAList.get(obCount).setY2(0);
                obstacleBList.get(obCount).setY1(-180);
                obstacleBList.get(obCount).setY2(0);
            } else if (obCount == 1) { // second obstacle position
                obstacleAList.get(obCount).setY1(-720);
                obstacleAList.get(obCount).setY2(-540);
                obstacleBList.get(obCount).setY1(-720);
                obstacleBList.get(obCount).setY2(-540);
            } else if (obCount == 2) { // third obstacle position
                obstacleAList.get(obCount).setY1(-1260);
                obstacleAList.get(obCount).setY2(-1080);
                obstacleBList.get(obCount).setY1(-1260);
                obstacleBList.get(obCount).setY2(-1080);
            } else { // fourth obstacle position;
                obstacleAList.get(obCount).setY1(-1800);
                obstacleAList.get(obCount).setY2(-1620);
                obstacleBList.get(obCount).setY1(-1800);
                obstacleBList.get(obCount).setY2(-1620);
            }

            int type = rand.nextInt(3);
            while (type == prevObType) {
                type = rand.nextInt(3);
            }

            int AX1; int BX1;
            int AX2; int BX2;

            if (type == 0) { // mid gap
                AX1 = -1080; BX1 = 702;
                AX2 = 378; BX2 = 2160;
                obstacleAList.get(obCount).setX1(AX1);
                obstacleAList.get(obCount).setX2(AX2);
                obstacleBList.get(obCount).setX1(BX1);
                obstacleBList.get(obCount).setX2(BX2);
                prevObType = 0;
            }
            else if (type == 1){ // left gap
                AX1 = -1080; BX1 = 540;
                AX2 = 216; BX2 = 2160;
                obstacleAList.get(obCount).setX1(AX1);
                obstacleAList.get(obCount).setX2(AX2);
                obstacleBList.get(obCount).setX1(BX1);
                obstacleBList.get(obCount).setX2(BX2);
                prevObType = 1;
            }
            else if (type == 2){ // right gap
                AX1 = -1080; BX1 = 864;
                AX2 = 540; BX2 = 2160;
                obstacleAList.get(obCount).setX1(AX1);
                obstacleAList.get(obCount).setX2(AX2);
                obstacleBList.get(obCount).setX1(BX1);
                obstacleBList.get(obCount).setX2(BX2);
                prevObType = 2;
            }
        }
        score = 0;
        colourInt = 0;
    }
}