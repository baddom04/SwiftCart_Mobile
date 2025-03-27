using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ShoppingList.Utils
{
    public static class FuzzyMatcher
    {
        private const double SCORE_CONTINUE_MATCH = 1.0;
        private const double SCORE_SPACE_WORD_JUMP = 0.9;
        private const double SCORE_NON_SPACE_WORD_JUMP = 0.8;
        private const double SCORE_CHARACTER_JUMP = 0.17;
        private const double SCORE_TRANSPOSITION = 0.1;
        private const double PENALTY_SKIPPED = 0.999;
        private const double PENALTY_CASE_MISMATCH = 0.9999;
        private const double PENALTY_NOT_COMPLETE = 0.99;

        private static readonly Regex IS_GAP_REGEXP = new Regex(@"[\\/_+.#""@[({&]", RegexOptions.Compiled);
        private static readonly Regex COUNT_GAPS_REGEXP = new Regex(@"[\\/_+.#""@[({&]", RegexOptions.Compiled);
        private static readonly Regex IS_SPACE_REGEXP = new Regex(@"[\s-]", RegexOptions.Compiled);
        private static readonly Regex COUNT_SPACE_REGEXP = new Regex(@"[\s-]", RegexOptions.Compiled);
        private static readonly Regex DIACRITICS_REGEX = new Regex(@"\p{IsCombiningDiacriticalMarks}+", RegexOptions.Compiled);

        public static double CommandScore(string input, string abbreviation)
        {
            ArgumentNullException.ThrowIfNull(input);
            ArgumentNullException.ThrowIfNull(abbreviation);

            string formattedInput = FormatInput(input);
            string formattedAbbreviation = FormatInput(abbreviation);

            var memo = new Dictionary<string, double>();

            return CommandScoreInner(input, abbreviation, formattedInput, formattedAbbreviation, 0, 0, memo);
        }

        private static double CommandScoreInner(
            string original,
            string abbreviation,
            string lowerOriginal,
            string lowerAbbreviation,
            int strIndex,
            int abbrIndex,
            Dictionary<string, double> memo)
        {
            if (abbrIndex == lowerAbbreviation.Length)
            {
                return (strIndex == lowerOriginal.Length)
                    ? SCORE_CONTINUE_MATCH
                    : PENALTY_NOT_COMPLETE;
            }

            string memoKey = $"{strIndex},{abbrIndex}";
            if (memo.TryGetValue(memoKey, out double memoizedResult))
            {
                return memoizedResult;
            }

            char abbrChar = lowerAbbreviation[abbrIndex];
            double highScore = 0.0;
            int index = lowerOriginal.IndexOf(abbrChar, strIndex);

            while (index >= 0)
            {
                double score = CommandScoreInner(original, abbreviation, lowerOriginal, lowerAbbreviation, index + 1, abbrIndex + 1, memo);

                if (score > 0)
                {
                    if (index == strIndex)
                    {
                        score *= SCORE_CONTINUE_MATCH;
                    }
                    else if (index - 1 >= 0 && IS_GAP_REGEXP.IsMatch(original[index - 1].ToString()))
                    {
                        score *= SCORE_NON_SPACE_WORD_JUMP;
                        string segment = original.Substring(strIndex, index - strIndex - 1);
                        var gapMatches = COUNT_GAPS_REGEXP.Matches(segment);
                        if (gapMatches.Count > 0 && strIndex > 0)
                        {
                            score *= Math.Pow(PENALTY_SKIPPED, gapMatches.Count);
                        }
                    }
                    else if (index - 1 >= 0 && IS_SPACE_REGEXP.IsMatch(original[index - 1].ToString()))
                    {
                        score *= SCORE_SPACE_WORD_JUMP;
                        string segment = original.Substring(strIndex, index - strIndex - 1);
                        var spaceMatches = COUNT_SPACE_REGEXP.Matches(segment);
                        if (spaceMatches.Count > 0 && strIndex > 0)
                        {
                            score *= Math.Pow(PENALTY_SKIPPED, spaceMatches.Count);
                        }
                    }
                    else
                    {
                        score *= SCORE_CHARACTER_JUMP;
                        if (strIndex > 0)
                        {
                            score *= Math.Pow(PENALTY_SKIPPED, index - strIndex);
                        }
                    }

                    if (original[index] != abbreviation[abbrIndex])
                    {
                        score *= PENALTY_CASE_MISMATCH;
                    }
                }

                if (index - 1 >= 0 && abbrIndex + 1 < lowerAbbreviation.Length)
                {
                    bool transpositionCandidate =
                        (score < SCORE_TRANSPOSITION && lowerOriginal[index - 1] == lowerAbbreviation[abbrIndex + 1]) ||
                        (abbrIndex + 1 < lowerAbbreviation.Length &&
                         lowerAbbreviation[abbrIndex + 1] == lowerAbbreviation[abbrIndex] &&
                         lowerOriginal[index - 1] != lowerAbbreviation[abbrIndex]);
                    if (transpositionCandidate)
                    {
                        double transposedScore = CommandScoreInner(
                            original, abbreviation, lowerOriginal, lowerAbbreviation,
                            index + 1, abbrIndex + 2, memo);
                        if (transposedScore * SCORE_TRANSPOSITION > score)
                        {
                            score = transposedScore * SCORE_TRANSPOSITION;
                        }
                    }
                }

                if (score > highScore)
                {
                    highScore = score;
                }

                index = lowerOriginal.IndexOf(abbrChar, index + 1);
            }

            memo[memoKey] = highScore;
            return highScore;
        }

        private static string FormatInput(string input)
        {
            string result = input.ToLowerInvariant();

            result = COUNT_SPACE_REGEXP.Replace(result, " ");

            string normalized = result.Normalize(NormalizationForm.FormD);

            string withoutDiacritics = DIACRITICS_REGEX.Replace(normalized, string.Empty);

            return withoutDiacritics.Normalize(NormalizationForm.FormC);
        }
    }
}
