echo "ビルド開始..."
export UNITY_APP_PATH="/Applications/Unity/Hub/Editor/6000.0.37f1/Unity.app/Contents/MacOS/Unity"
export UNITY_PROJECT_PATH="../../../../URP"
export UNITY_BUILDFUNC_NAME="BuildScript.AndroidAABBuild"
export BUILD_OUTPUT_PATH="$UNITY_PROJECT_PATH/Build/"
export BUILD_DATE=$(date +"%Y%m%d_%H%M%S")
echo "Unityビルド実行中..."
$UNITY_APP_PATH -batchmode \
  -quit \
  -projectPath $UNITY_PROJECT_PATH \
  -executeMethod $UNITY_BUILDFUNC_NAME \
  -logFile ./buildAAB.log
  
if [ $? -eq 0 ]; then
  echo "ビルド成功。"
  echo "名前変更"
  
  mv $BUILD_OUTPUT_PATH".aab" $BUILD_OUTPUT_PATH"_${BUILD_DATE}.aab"
  rclone copy $BUILD_OUTPUT_PATH"_${BUILD_DATE}.apk" sokigb:/Builds -P
  echo "アップロード完了！"
else
  echo "ビルド失敗。ログを確認してください。"
fi
