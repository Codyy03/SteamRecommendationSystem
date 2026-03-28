export async function getPythonRecomentationGamesByName(gameName: string) {
    const response = await fetch(`https://localhost:7179/api/PythonRecommendation/pythonRecommendation?gameName=${gameName}`)
}