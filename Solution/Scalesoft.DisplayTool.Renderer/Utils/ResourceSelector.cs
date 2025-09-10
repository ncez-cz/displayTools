using Scalesoft.DisplayTool.Shared.DocumentNavigation;

namespace Scalesoft.DisplayTool.Renderer.Utils;
public static class ResourceSelector
{
    /// <summary>
    /// Selects the most appropriate FHIR resource index based on use types and date validity.
    /// The selection process prioritizes resources in the following order:
    /// 1. Matches the prioritized use types (in the order provided)
    /// 2. Has a valid end date (either null or a future date)
    /// 3. Resources that don't match any priority types are considered last
    /// </summary>
    /// <param name="navigators">List of XML navigators containing FHIR resources to evaluate</param>
    /// <param name="prioritizedUseTypes">Array of use types in priority order. Resources are matched against these types 
    /// in sequence, with earlier matches taking precedence</param>
    /// <param name="usePath">XPath to the 'use' value in the XML document. Defaults to "f:use/@value"</param>
    /// <param name="endPath">XPath to the period end date in the XML document. Defaults to "f:period/f:end/@value"</param>
    /// <returns>
    /// Index (1-based) of the selected resource. Returns 1 if:
    /// - The input list contains only one item
    /// - No valid candidates are found
    /// </returns>
     public static int SelectPreferredResourceIndex(List<XmlDocumentNavigator> navigators, string?[] prioritizedUseTypes,
             string usePath = "f:use/@value",
             string endPath = "f:period/f:end/@value")
         {
             var navigatorCount = navigators.Count;
             var candidatesByPriority = new Dictionary<int, (int index, bool validEndDate)>();
             
             if (navigatorCount <= 1) 
                 return 1;
             
             for (var i = 0; i < navigatorCount; i++)
             {
                 var useNode = navigators[i].SelectSingleNode(usePath).Node;
                 var useValue = useNode?.Value;
         
                 var hasValidEndDate = ResourceValidation.IsNodeCurrent(navigators[i], endPath, usePath);

                 var foundMatch = false;
                 for (var typeIndex = 0; typeIndex < prioritizedUseTypes.Length; typeIndex++)
                 {
                     var type = prioritizedUseTypes[typeIndex];
                     var isMatch = useValue == type;

                     if (!isMatch) continue;
                     
                     foundMatch = true;
                     
                     if (!candidatesByPriority.ContainsKey(typeIndex))
                     {
                         candidatesByPriority[typeIndex] = (i+1, hasValidEndDate);
                     }
                     else if (candidatesByPriority.TryGetValue(typeIndex, out var value) && !value.validEndDate && hasValidEndDate)
                     {
                         candidatesByPriority[typeIndex] = (i+1, hasValidEndDate);
                     }
 
                     break;
                 }

                 if (foundMatch) continue;
                 
                 //If the provided useType doesn't match any of the prioritizedUseTypes, it is still added to the array but to an index where it's priority is the lowest.
                 var nonMatchPriority = prioritizedUseTypes.Length + i;
                 candidatesByPriority[nonMatchPriority] = (i+1, hasValidEndDate);
             }
             
             return candidatesByPriority.Count > 0 ? candidatesByPriority[candidatesByPriority.Keys.Min()].index : throw new InvalidOperationException();
         }   
}
