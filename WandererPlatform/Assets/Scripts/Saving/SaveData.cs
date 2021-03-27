using System;

[Serializable]
public class SaveData {
    
    private static SaveData _current;
    public static SaveData current {
        get {
            if(_current == null) {
                _current = new SaveData();
            }
            return _current;
        }
        set {
            if(value != null) {
                _current = value;
            }
        }
    }
    
    //public int highScore;
    public int currentScore;
    public int coins;
    public bool hasGem;
    public int level = 1;

    public void Reset() {
        currentScore = 0;
        coins = 0;
        hasGem = false;
        level = 2;
    }
}

