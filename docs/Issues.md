# To Fix

## バグ

### 重要度 高

- ~敵にアルファベットがぶつかったときそのアルファベットを2回以上受け取ってしまう~ -> Alphabetの方にフラグを追加し, EnemyにぶつかるとそのフラグをFalseする, フラグがFalseの際はOnCollisionEnterはすぐにreturnするようにして修正
- ~敵が消滅の条件を満たしてから消えるまでの間で動いてしまう~ -> 単語ができたらHPを減らし, HP0でDieStateに移行するようにした

### 重要度 中

- 壁の向こう側が見える

## 仕様変更

- アルファベット切り替えが、今のままだとおそらくアルファベットが増えると冗長になる(切り替えに時間かかる)ので早くできるように or 別の入力を考える
- VoiceInputで使っているUnityEngine.Windows.SpeechはWebGL出力不可なので音声処理(というか入力)をやりたいなら別の手立てが必要

## コード(リファクタリング)

### 直すべき

- movementの基底クラスをつくり、継承させる
- enemyのmovementの方に、Navmeshによる経路設計のコードを関数として書くようにする -> StateMachineでは, _movementのメソッドを呼び出す形に(できればenemyのコードの方にMoveがあると良い)
- ResourcesLoaderの活用
- クラス、メソッド、変数名で怪しいものがあるので見直してRename

### できれば直したい

- そもそもアセット読み込みをResourcesではなく、AssetBundleにする
- Scriptable Objectの活用
- 階層ステートマシン・平行ステートマシンの活用(DashとかTiredの管理に使える)
