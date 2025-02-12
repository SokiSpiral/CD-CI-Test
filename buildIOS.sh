echo "ビルド開始..."
export UNITY_APP_PATH="/Applications/Unity/Hub/Editor/6000.0.37f1/Unity.app/Contents/MacOS/Unity"
export UNITY_PROJECT_PATH="/Users/kitamura/Documents/CD-CI-Test/URP"
export UNITY_BUILDFUNC_NAME="BuildScript.BuildiOS"
export BUILD_OUTPUT_PATH="/Users/kitamura/Documents/CD-CI-Test/URP/Build/build"
export BUILD_DATE=$(date +"%Y%m%d_%H%M%S")
echo "Unityビルド実行中..."
$UNITY_APP_PATH -batchmode \
  -quit \
  -projectPath $UNITY_PROJECT_PATH \
  -executeMethod $UNITY_BUILDFUNC_NAME \
  -logFile ./buildIOS.log
  
if [ $? -eq 0 ]; then
  echo "ビルド成功。"
else
  echo "ビルド失敗。ログを確認してください。"
fi
