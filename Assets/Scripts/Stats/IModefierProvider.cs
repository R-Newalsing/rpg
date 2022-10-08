using System.Collections.Generic;

namespace RPG.Stats{
interface IModefierProvider {
    IEnumerable<float> GetAdditiveModefiers(Stat stat);
    IEnumerable<float> GetPercentageModefiers(Stat stat);
}}