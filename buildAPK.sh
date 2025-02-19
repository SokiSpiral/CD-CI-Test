echo "フォルダ移動"
cd ../../../../ || { echo "移動に失敗"; exit 1; }
echo "Current Directory $PWD"

git pull origin main || { echo "pullに失敗"; exit 1; }

echo "ビルド開始..."
export UNITY_APP_PATH="/Applications/Unity/Hub/Editor/6000.0.37f1/Unity.app/Contents/MacOS/Unity"
export UNITY_PROJECT_PATH="./URP"
export UNITY_BUILDFUNC_NAME="BuildScript.AndroidAPKBuild"
export BUILD_OUTPUT_PATH="$UNITY_PROJECT_PATH/Build/"
export BUILD_DATE=$(date +"%Y%m%d_%H%M%S")

export FIREBASE_APP_DISTRIBUTION_APP_ID="1:196709384800:android:7fc668a7754a38ed993905"
echo "Unityビルド実行中..."
$UNITY_APP_PATH -batchmode \
  -quit \
  -projectPath $UNITY_PROJECT_PATH \
  -executeMethod $UNITY_BUILDFUNC_NAME \
  -logFile ./buildAPK.log
  
if [ $? -eq 0 ]; then
  echo "ビルド成功。"
else
  echo "ビルド失敗。ログを確認してください。"
  exit 1
fi

echo "名前変更"
mv $BUILD_OUTPUT_PATH"build.apk" $BUILD_OUTPUT_PATH"build_${BUILD_DATE}.apk" || { echo "名前変更に失敗"; exit 1; }

echo "Firebase App Distributionへアップロード"
firebase appdistribution:distribute $BUILD_OUTPUT_PATH"build_${BUILD_DATE}.apk" --app $FIREBASE_APP_DISTRIBUTION_APP_ID --groups tester　|| { echo "uploadに失敗"; exit 1; }
echo "アップロード完了！"
