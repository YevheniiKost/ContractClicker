using UnityEditor;
using UnityEngine;
using System.Linq;

using YeKostenko.ContractClicker.Data.Game;
using YeKostenko.ContractClicker.Data.Modifiers;

namespace YeKostenko.ContractClicker.Editor.Modifiers
{
    /// <summary>
    /// EditorWindow для швидкого огляду всіх Modifier Sets з бази даних
    /// </summary>
    public class ModifierSetBrowserWindow : EditorWindow
    {
        private ModifierDatabase _database;
        private Vector2 _scrollPosition;
        private ContractDifficulty _filterDifficulty = (ContractDifficulty)(0); // All
        private int _filterMinLevel = 0;
        private string _searchQuery = "";
        private bool _showOnlyWithModifiers = false;

        [MenuItem("Window/Contract Clicker/Modifier Set Browser")]
        public static void ShowWindow()
        {
            var window = GetWindow<ModifierSetBrowserWindow>("Modifier Sets");
            window.minSize = new Vector2(600, 400);
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawFilters();

            EditorGUILayout.Space(10);

            if (_database == null)
            {
                DrawNoDatabaseMessage();
                return;
            }

            DrawModifierSetsList();
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            EditorGUILayout.LabelField("Modifier Set Browser", EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton, GUILayout.Width(60)))
            {
                Repaint();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawFilters()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            // Database selector
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Database:", GUILayout.Width(70));
            _database = (ModifierDatabase)EditorGUILayout.ObjectField(_database, typeof(ModifierDatabase), false);
            EditorGUILayout.EndHorizontal();

            if (_database != null)
            {
                EditorGUILayout.Space(5);

                // Search
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Search:", GUILayout.Width(70));
                _searchQuery = EditorGUILayout.TextField(_searchQuery);
                if (GUILayout.Button("✕", GUILayout.Width(25)))
                {
                    _searchQuery = "";
                    GUI.FocusControl(null);
                }
                EditorGUILayout.EndHorizontal();

                // Filters row
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Filters:", GUILayout.Width(70));

                // Difficulty filter
                var allDifficulties = new[] { "All" }.Concat(System.Enum.GetNames(typeof(ContractDifficulty))).ToArray();
                int selectedIndex = (int)_filterDifficulty + 1;
                selectedIndex = EditorGUILayout.Popup(selectedIndex, allDifficulties, GUILayout.Width(100));
                _filterDifficulty = (ContractDifficulty)(selectedIndex - 1);

                // Level filter
                EditorGUILayout.LabelField("Min Level:", GUILayout.Width(65));
                _filterMinLevel = EditorGUILayout.IntField(_filterMinLevel, GUILayout.Width(40));

                // Only with modifiers
                _showOnlyWithModifiers = EditorGUILayout.ToggleLeft("Only Active", _showOnlyWithModifiers, GUILayout.Width(90));

                if (GUILayout.Button("Clear Filters", GUILayout.Width(90)))
                {
                    _filterDifficulty = (ContractDifficulty)(0);
                    _filterMinLevel = 0;
                    _searchQuery = "";
                    _showOnlyWithModifiers = false;
                }

                EditorGUILayout.EndHorizontal();

                // Stats
                EditorGUILayout.BeginHorizontal();
                var totalSets = _database.ModifierSets.Count;
                var filteredSets = GetFilteredSets().Count();
                EditorGUILayout.LabelField($"Showing {filteredSets} of {totalSets} sets", EditorStyles.miniLabel);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawNoDatabaseMessage()
        {
            EditorGUILayout.HelpBox("Please assign a Modifier Database to view sets.", MessageType.Info);

            if (GUILayout.Button("Find Modifier Database in Project"))
            {
                var databases = AssetDatabase.FindAssets("t:ModifierDatabase")
                    .Select(AssetDatabase.GUIDToAssetPath)
                    .Select(AssetDatabase.LoadAssetAtPath<ModifierDatabase>)
                    .Where(db => db != null)
                    .ToArray();

                if (databases.Length > 0)
                {
                    _database = databases[0];
                    Repaint();
                }
                else
                {
                    EditorUtility.DisplayDialog("Not Found",
                        "No Modifier Database found in project. Please create one first.", "OK");
                }
            }
        }

        private void DrawModifierSetsList()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            var filteredSets = GetFilteredSets().ToArray();

            if (filteredSets.Length == 0)
            {
                EditorGUILayout.HelpBox("No modifier sets match the current filters.", MessageType.Info);
            }
            else
            {
                // Group by difficulty
                var groupedByDifficulty = filteredSets.GroupBy(s => s.Difficulty).OrderBy(g => g.Key);

                foreach (var group in groupedByDifficulty)
                {
                    DrawDifficultyGroup(group.Key, group.ToArray());
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawDifficultyGroup(ContractDifficulty difficulty, ModifierSetConfig[] sets)
        {
            EditorGUILayout.Space(5);

            var difficultyColor = GetDifficultyColor(difficulty);
            var originalColor = GUI.backgroundColor;
            GUI.backgroundColor = difficultyColor;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUI.backgroundColor = originalColor;

            // Difficulty header
            var headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 13,
                normal = { textColor = difficultyColor }
            };
            EditorGUILayout.LabelField($"{difficulty} ({sets.Length})", headerStyle);

            EditorGUILayout.Space(3);

            foreach (var set in sets.OrderByDescending(s => s.SpawnChance))
            {
                DrawModifierSetCard(set);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawModifierSetCard(ModifierSetConfig set)
        {
            var cardColor = set.DifficultyColor;
            if (cardColor == Color.white)
            {
                cardColor = GetDifficultyColor(set.Difficulty);
            }
            cardColor.a = 0.3f;

            var originalColor = GUI.backgroundColor;
            GUI.backgroundColor = cardColor;

            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUI.backgroundColor = originalColor;

            // Left side - Info
            EditorGUILayout.BeginVertical(GUILayout.Width(250));

            // Name
            var nameStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 12
            };
            EditorGUILayout.LabelField(set.SetName, nameStyle);

            // Description
            if (!string.IsNullOrEmpty(set.SetDescription))
            {
                var descStyle = new GUIStyle(EditorStyles.miniLabel)
                {
                    wordWrap = true
                };
                EditorGUILayout.LabelField(set.SetDescription, descStyle, GUILayout.MaxWidth(240));
            }

            EditorGUILayout.EndVertical();

            // Middle - Stats
            EditorGUILayout.BeginVertical(GUILayout.Width(200));

            EditorGUILayout.BeginHorizontal();
            DrawMiniStat("Level:", $"≥{set.MinContractLevel}", new Color(0.5f, 0.7f, 1f));
            DrawMiniStat("Spawn:", $"{set.SpawnChance:F0}%", new Color(0.7f, 0.5f, 1f));
            EditorGUILayout.EndHorizontal();

            // Active modifiers count
            var activeCount = set.GetActiveModifierConfigs().Count();
            var countColor = activeCount > 0 ? new Color(0.3f, 0.8f, 0.3f) : Color.gray;
            var countStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                normal = { textColor = countColor },
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField($"{activeCount} active modifier(s)", countStyle);

            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            // Right side - Actions
            EditorGUILayout.BeginVertical(GUILayout.Width(100));

            if (GUILayout.Button("Select", GUILayout.Height(25)))
            {
                Selection.activeObject = set;
                EditorGUIUtility.PingObject(set);
            }

            if (GUILayout.Button("Duplicate", GUILayout.Height(20)))
            {
                DuplicateSet(set);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void DrawMiniStat(string label, string value, Color color)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(60));

            EditorGUILayout.LabelField(label, EditorStyles.miniLabel);

            var valueStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                normal = { textColor = color },
                fontStyle = FontStyle.Bold
            };
            EditorGUILayout.LabelField(value, valueStyle);

            EditorGUILayout.EndVertical();
        }

        private System.Collections.Generic.IEnumerable<ModifierSetConfig> GetFilteredSets()
        {
            var sets = _database.ModifierSets.Where(s => s != null);

            // Filter by difficulty
            if (_filterDifficulty >= 0)
            {
                sets = sets.Where(s => s.Difficulty == _filterDifficulty);
            }

            // Filter by level
            if (_filterMinLevel > 0)
            {
                sets = sets.Where(s => s.MinContractLevel >= _filterMinLevel);
            }

            // Filter by search query
            if (!string.IsNullOrEmpty(_searchQuery))
            {
                sets = sets.Where(s =>
                    s.SetName.IndexOf(_searchQuery, System.StringComparison.OrdinalIgnoreCase) >= 0 ||
                    (s.SetDescription != null && s.SetDescription.IndexOf(_searchQuery, System.StringComparison.OrdinalIgnoreCase) >= 0)
                );
            }

            // Filter by having modifiers
            if (_showOnlyWithModifiers)
            {
                sets = sets.Where(s => s.GetActiveModifierConfigs().Any());
            }

            return sets;
        }

        private void DuplicateSet(ModifierSetConfig original)
        {
            var path = AssetDatabase.GetAssetPath(original);
            var newPath = AssetDatabase.GenerateUniqueAssetPath(path);

            if (AssetDatabase.CopyAsset(path, newPath))
            {
                AssetDatabase.Refresh();
                var duplicate = AssetDatabase.LoadAssetAtPath<ModifierSetConfig>(newPath);
                Selection.activeObject = duplicate;
                EditorGUIUtility.PingObject(duplicate);
            }
        }

        private Color GetDifficultyColor(ContractDifficulty difficulty)
        {
            return difficulty switch
            {
                ContractDifficulty.Easy => new Color(0.6f, 0.9f, 0.6f),
                ContractDifficulty.Medium => new Color(0.6f, 0.8f, 1f),
                ContractDifficulty.Hard => new Color(0.9f, 0.3f, 0.3f),
                _ => Color.white
            };
        }
    }
}

