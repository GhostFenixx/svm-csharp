using Greed.Models;
using SPTarkov.Server.Core.Constants;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace ServerValueModifier.Sections
{
    internal class Advanced(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig)
    {
        public void ItemChangerSection()
        {
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;
            Dictionary<MongoId, TemplateItem> items = databaseService.GetItems();

            //Mailbox related
            var coreConfig = configServer.GetConfig<CoreConfig>();
            if (svmconfig.Custom.DisableCommando) // Made it this way in case other mods will override these fields, like Fika
            {
                coreConfig.Features.ChatbotFeatures.EnabledBots["6723fd51c5924c57ce0ca01e"] = false;
            }
            if (svmconfig.Custom.DisableSPTFriend) 
            {
                coreConfig.Features.ChatbotFeatures.EnabledBots["6723fd51c5924c57ce0ca01f"] = false;
            }
            if (svmconfig.Custom.DisablePMCMessages) 
            {
                var chatConfig = configServer.GetConfig<PmcChatResponse>();
                chatConfig.Victim.ResponseChancePercent = 0;
                chatConfig.Killer.ResponseChancePercent = 0;
            }
            //IIC
            if (svmconfig.Custom.IDChanger)
            {
                logger.Info("[SVM] Custom Properties is loading");
                if (svmconfig.Custom.IDDefault.Length > 0 && svmconfig.Custom.IDDefault != "")
                {
                    string[] defaultList = svmconfig.Custom.IDDefault.Split("\r\n");
                    foreach (string line in defaultList)
                    {
                        if (!line.StartsWith("#") && !line.StartsWith("//") && line.Contains(':'))
                        {
                            string[] variables = line.Split(":");
                            Fixfields(variables);
                            logger.Info("Default: " + line);
                            IDChanger(variables, items);
                        }
                    }
                }
                if (svmconfig.Custom.IDParent.Length > 0 && svmconfig.Custom.IDParent != "")
                {
                    string[] parentlist = svmconfig.Custom.IDParent.Split("\r\n");
                    foreach (string line in parentlist)
                    {
                        List<string> idarray = [];
                        if (!line.StartsWith("#") && !line.StartsWith("//") && line.Contains(':') && line.Length > 1)
                        {
                            var variables = line.Split(":");
                            Fixfields(variables);
                            logger.Info("[SVM] Parent: " + line.ToString());
                            logger.Debug("[SVM] Affected by parent: ");
                            foreach (var ids in items)
                            {
                                if (ids.Value.Parent == variables[0])
                                {
                                    logger.Debug(ids.Value.Name.ToString());
                                    idarray.Add(ids.Value.Id.ToString());
                                }
                            }
                            foreach (var result in idarray)
                            {

                                variables[0] = result;
                                IDChanger(variables, items);
                            }
                        }
                    }
                }
                if (svmconfig.Custom.IDPrice.Length > 0 && svmconfig.Custom.IDPrice != "")
                {
                    string[] prices = svmconfig.Custom.IDPrice.Split("\r\n");
                    foreach (string line in prices)
                    {
                        if (!line.StartsWith("#") && !line.StartsWith("//"))
                        {
                            string[] variables = line.Split(":");
                            logger.Info("Price: " + line);
                            PriceChange(variables);
                        }
                    }
                }
                logger.Success("[SVM] Custom properties successfully loaded");
            }
            string[] offers = svmconfig.Custom.AddTraderAssort.Split("\r\n");
            var traders = databaseService.GetTraders();
            foreach (string line in offers)
            {
                if (!line.StartsWith("#") && !line.StartsWith("//") && line.Contains(':') && svmconfig.Custom.AddTraderAssort != "")
                {
                    string[] variables = line.Split(":");
                    MongoId uid = new();
                    variables[0] = variables[0] switch
                    {
                        "Therapist" => "54cb57776803fa99248b456e",
                        "Prapor" => "54cb50c76803fa8b248b4571",
                        "Mechanic" => "5a7c2eca46aef81a7ca2145d",
                        "Ragman" => "5ac3b934156ae10c4430e83c",
                        "Jaeger" => "5c0647fdd443bc2504c2d371",
                        "Peacekeeper" => "5935c25fb3acc3127c3d8cd9",
                        "Ref" => "6617beeaa9cfa777ca915b7c",
                        "Skier" => "58330581ace78e27b8b10cee",
                        _ => variables[0]
                    };
                    variables[1] = variables[1] switch
                    {
                        "USD" => ItemTpl.MONEY_DOLLARS,
                        "RUB" => ItemTpl.MONEY_ROUBLES,
                        "EUR" => ItemTpl.MONEY_EUROS,
                        "GP" => ItemTpl.MONEY_GP_COIN,
                        _ => variables[1]
                    };
                    Item item = new()
                    {
                        Upd = new Upd
                        {
                            UnlimitedCount = true,
                            StackObjectsCount = 99999
                        },
                        Id = uid,
                        Template = variables[3],
                        ParentId = "hideout",
                        SlotId = "hideout"
                    };
                    List<List<BarterScheme>> barterScheme = new() // Holy shit BSG.
                    {
                        new List<BarterScheme>
                        {
                            new BarterScheme
                            {
                                Count = Convert.ToDouble(variables[2]),
                                Template = variables[1]
                            }
                        }
                    };
                    traders[variables[0]].Assort.Items.Add(item);
                    traders[variables[0]].Assort.BarterScheme.Add(uid, barterScheme);
                    traders[variables[0]].Assort.LoyalLevelItems.Add(uid, Convert.ToInt32(variables[4]));
                }
            }
            if (svmconfig.Custom.FleaMultID != "" && svmconfig.Custom.FleaMultID.Length > 1)
            {
                try
                {
                    string[] IDlist = svmconfig.Custom.FleaMultID.Split("\r\n");
                    var fleaconfig = configServer.GetConfig<RagfairConfig>();
                    fleaconfig.Dynamic.GenerateBaseFleaPrices.ItemTplMultiplierOverride = [];
                    foreach (string line in IDlist)
                    {
                        if (!line.StartsWith("#") && !line.StartsWith("//") && line.Contains(':'))
                        {
                            string[] variables = line.Split(":");
                            if (variables[1].Contains(','))
                            {
                                variables[1] = variables[1].Replace(',', '.');
                            }
                            fleaconfig.Dynamic.GenerateBaseFleaPrices.ItemTplMultiplierOverride.Add(variables[0], Convert.ToDouble(variables[1]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("[SVM] Advanced Features - Flea Market - Item's multipliers: Syntax error, read about the error below \n\n" + ex);
                }
            }
            //Blacklist, will rewrite it later
            if (svmconfig.Custom.Blacklist != "" && svmconfig.Custom.Blacklist.Length > 1)
            {
                TraderConfig traderConfig = configServer.GetConfig<TraderConfig>();
                foreach (string item in svmconfig.Custom.Blacklist.Split("\r\n"))
                {
                    traderConfig.Fence.Blacklist.Add(item);
                }
            }
        }

        public void Fixfields(string[] variables)
        {
            for (int i = 0; i < variables.Length; i++)
            {
                if (!string.IsNullOrEmpty(variables[i]) && char.IsLower(variables[i][0]))
                {
                    variables[i] = char.ToUpper(variables[i][0]) + variables[i].Substring(1); //Attempt to compensate that we require upper case while Item Finder provide lowercase.
                }
                if (variables[i].Contains("_props"))
                {
                    variables[i] = "Properties";
                }
            }
        }
        public void PriceChange(string[] variables)
        {
            var handbook = databaseService.GetHandbook();
            handbook.Items.ForEach(item =>
            {
                if (item.Id == variables[0])
                {
                    double value = Convert.ToDouble(variables[2]);
                    double? result = variables[1] switch
                    {
                        "+" => item.Price + value,
                        "-" => item.Price - value,
                        "*" => item.Price * value,
                        "/" => item.Price / value,
                        "=" => item.Price = value,
                        _ => throw new NotSupportedException($"Operand '{variables[1]}' not supported") //Should i even have these checks?
                    };
                    item.Price = result;
                }
            });
        }
        // Individual Item Changer, IIC - all the magic goes here, it cycles through lists, makes lines, splits lines into variables, read through them, applies whether directly or after basic math option.
        // Basically home cooked interpreter with own syntax that utilises reflection to apply changes to Items.json
        public void IDChanger(string[] variables, Dictionary<MongoId, TemplateItem> items)
        {
            try
            {
                //Stripping last number as field we replace/expression with
                string last = variables.Last();
                //Removing first because we define ID in method
                string[] path = variables.Skip(1).Take(variables.Length - 2).ToArray();
                SetNestedValue(items[variables[0]].Properties, path, last);
            }
            catch (Exception e)
            {
                logger.Error("[SVM] INVENTORY AND ITEMS - Custom properties: failed to load, error of the code:\n" + e);
            }
        }

        //This is scary
        //What is does is cycle through layers, checking whether it's operand, property or dataset/collection/dictionary/hashset, then adds stringValue on last member of Path and applies it to root which is our items
        public static void SetNestedValue(object obj, string[] path, string stringValue)
        {
            object current = obj;

            // Detect operator at last element
            string? operation = null;
            if (path.Length > 1 && new[] { "+", "-", "*", "/", "=" }.Contains(path[^1]))
            {
                operation = path[^1];
                path = path.Take(path.Length - 1).ToArray();
            }

            for (int i = 0; i < path.Length; i++)
            {
                string part = path[i];
                bool isLast = i == path.Length - 1;

                if (current == null)
                    throw new NullReferenceException($"Null value while traversing path at '{part}'.");

                Type currentType = current.GetType();

                // --- Dictionaries ---
                if (current is IDictionary dict)
                {
                    Type keyType = currentType.GetGenericArguments()[0];
                    Type valType = currentType.GetGenericArguments()[1];
                    object key = keyType.IsEnum ? Enum.Parse(keyType, part, true) : Convert.ChangeType(part, keyType);

                    if (!dict.Contains(key))
                        throw new KeyNotFoundException($"Key '{part}' not found in dictionary.");

                    if (isLast)
                    {
                        object oldValue = dict[key]!;
                        object newValue = ConvertStringToTypeOrCollection(stringValue, valType, oldValue, operation);
                        dict[key] = newValue;
                        return;
                    }

                    current = dict[key];
                    continue;
                }

                // --- IEnumerable / Array / List / HashSet ---
                if (typeof(IEnumerable).IsAssignableFrom(currentType) && currentType != typeof(string))
                {
                    Type elementType = currentType.IsArray
                        ? currentType.GetElementType()!
                        : currentType.GetGenericArguments().FirstOrDefault() ?? typeof(object);

                    if (!int.TryParse(part, out int index))
                        throw new InvalidOperationException($"'{part}' is not a valid index for IEnumerable.");

                    var items = ((IEnumerable)current).Cast<object>().ToList();
                    if (index < 0 || index >= items.Count)
                        throw new IndexOutOfRangeException($"Index {index} out of range.");

                    if (isLast)
                    {
                        object oldValue = items[index];
                        object newValue = ConvertStringToTypeOrCollection(stringValue, elementType, oldValue, operation);

                        // Append/replace properly for HashSet, IList, Array
                        if (IsHashSet(oldValue))
                        {
                            var addMethod = oldValue.GetType().GetMethod("Add")!;
                            if (newValue is IEnumerable enumerable)
                            {
                                foreach (var e in enumerable) addMethod.Invoke(oldValue, new[] { e });
                            }
                            else
                            {
                                addMethod.Invoke(oldValue, new[] { newValue });
                            }
                        }
                        else if (oldValue is IList list)
                        {
                            if (operation == "+")
                            {
                                if (newValue is IEnumerable enumerable)
                                    foreach (var e in enumerable) list.Add(e);
                                else list.Add(newValue);
                            }
                            else
                            {
                                list.Clear();
                                if (newValue is IEnumerable enumerable)
                                    foreach (var e in enumerable) list.Add(e);
                                else list.Add(newValue);
                            }
                        }
                        else if (currentType.IsArray)
                        {
                            ((Array)current).SetValue(newValue, index);
                        }
                        else
                        {
                            SetMemberValue(current, GetMember(current, part), newValue);
                        }

                        return;
                    }

                    current = items[index];
                    continue;
                }

                // --- Property / Field ---
                var memberInfo = GetMember(current, part);

                if (isLast)
                {
                    Type targetType = memberInfo is PropertyInfo pi ? pi.PropertyType : ((FieldInfo)memberInfo).FieldType;
                    object oldValue = memberInfo is PropertyInfo pi2 ? pi2.GetValue(current)! : ((FieldInfo)memberInfo).GetValue(current)!;

                    object newValue = ConvertStringToTypeOrCollection(stringValue, targetType, oldValue, operation);
                    SetMemberValue(current, memberInfo, newValue);
                    return;
                }

                current = memberInfo is PropertyInfo pi3 ? pi3.GetValue(current)! : ((FieldInfo)memberInfo).GetValue(current)!;
            }
        }

        private static bool IsHashSet(object value)
        {
            if (value == null) return false;
            var type = value.GetType();
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>);
        }

        private static object ConvertStringToTypeOrCollection(string strValue, Type targetType, object? existingValue = null, string? operation = null)
        {
            // Collection handling
            if (typeof(IEnumerable).IsAssignableFrom(targetType) && targetType != typeof(string)
                && strValue.Trim().StartsWith("[") && strValue.Trim().EndsWith("]"))
            {
                Type elementType = targetType.IsArray
                    ? targetType.GetElementType()!
                    : targetType.GetGenericArguments().FirstOrDefault() ?? typeof(object);

                var newElements = strValue.Trim('[', ']')
                                          .Split(',')
                                          .Select(s => ConvertStringToType(s.Trim(), elementType))
                                          .ToList();

                if (operation == "+")
                {
                    if (existingValue == null)
                        existingValue = CreateEmptyCollection(targetType, elementType);

                    if (IsHashSet(existingValue))
                    {
                        var addMethod = existingValue.GetType().GetMethod("Add")!;
                        foreach (var e in newElements) addMethod.Invoke(existingValue, new[] { e });
                        return existingValue;
                    }
                    else if (existingValue is IList list)
                    {
                        foreach (var e in newElements) list.Add(e);
                        return existingValue;
                    }
                    else if (existingValue.GetType().IsArray)
                    {
                        var arr = (Array)existingValue;
                        var newArr = Array.CreateInstance(elementType, arr.Length + newElements.Count);
                        arr.CopyTo(newArr, 0);
                        for (int j = 0; j < newElements.Count; j++) newArr.SetValue(newElements[j], arr.Length + j);
                        return newArr;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cannot add elements to {existingValue.GetType()}");
                    }
                }
                else // "=" or null operation => replace
                {
                    return CreateCollectionFromElements(targetType, elementType, newElements);
                }
            }

            // Primitive / Enum / string
            object converted = ConvertStringToType(strValue, targetType);
            if (operation != null && operation != "=")
            {
                converted = ApplyOperation(existingValue!, converted, operation);
            }
            return converted;
        }

        private static object CreateEmptyCollection(Type targetType, Type elementType)
        {
            Type concreteType = GetConcreteCollectionType(targetType, elementType);

            if (concreteType.IsArray)
                return Array.CreateInstance(elementType, 0);

            return Activator.CreateInstance(concreteType)!;
        }

        private static object CreateCollectionFromElements(Type targetType, Type elementType, List<object> elements)
        {
            Type concreteType = GetConcreteCollectionType(targetType, elementType);

            if (concreteType.IsArray)
            {
                var arr = Array.CreateInstance(elementType, elements.Count);
                for (int i = 0; i < elements.Count; i++) arr.SetValue(elements[i], i);
                return arr;
            }

            if (concreteType.IsGenericType && concreteType.GetGenericTypeDefinition() == typeof(HashSet<>))
            {
                var hashSet = Activator.CreateInstance(concreteType)!;
                var addMethod = hashSet.GetType().GetMethod("Add")!;
                foreach (var e in elements) addMethod.Invoke(hashSet, new[] { e });
                return hashSet;
            }

            var coll = (IList)Activator.CreateInstance(concreteType)!;
            foreach (var e in elements) coll.Add(e);
            return coll;
        }

        private static Type GetConcreteCollectionType(Type targetType, Type elementType)
        {
            Type concreteType = targetType;

            if (targetType.IsInterface || targetType.IsAbstract)
            {
                if (typeof(IEnumerable<>).MakeGenericType(elementType).IsAssignableFrom(targetType) ||
                    typeof(IList<>).MakeGenericType(elementType).IsAssignableFrom(targetType) ||
                    typeof(ICollection<>).MakeGenericType(elementType).IsAssignableFrom(targetType))
                {
                    concreteType = typeof(List<>).MakeGenericType(elementType);
                }
                else if (typeof(HashSet<>).MakeGenericType(elementType).IsAssignableFrom(targetType))
                {
                    concreteType = typeof(HashSet<>).MakeGenericType(elementType);
                }
                else
                {
                    throw new InvalidOperationException($"Cannot create collection for abstract/interface type {targetType}");
                }
            }

            return concreteType;
        }

        private static MemberInfo GetMember(object obj, string name)
        {
            var type = obj.GetType();
            var member = (MemberInfo?)type.GetProperty(name) ?? type.GetField(name) as MemberInfo;
            if (member == null)
                throw new MissingMemberException($"Member '{name}' not found in {type.Name}");
            return member;
        }

        private static void SetMemberValue(object obj, MemberInfo member, object value)
        {
            if (member is PropertyInfo pi) pi.SetValue(obj, value);
            else if (member is FieldInfo fi) fi.SetValue(obj, value);
            else throw new ArgumentException("Unsupported member type.");
        }

        private static object ConvertStringToType(string strValue, Type targetType)
        {
            if (targetType == typeof(string)) return strValue;

            var underlying = Nullable.GetUnderlyingType(targetType) ?? targetType;

            if (underlying.IsEnum)
                return Enum.Parse(underlying, strValue, true);
            if (underlying == typeof(MongoId))
                return (MongoId)strValue;

            if (underlying == typeof(Guid))
                return Guid.Parse(strValue);

            if (typeof(IConvertible).IsAssignableFrom(underlying))
                return Convert.ChangeType(strValue, underlying);

            var ctor = underlying.GetConstructor(new[] { typeof(string) });
            if (ctor != null)
                return ctor.Invoke(new object[] { strValue });

            throw new InvalidCastException($"Cannot convert string '{strValue}' to {underlying.FullName}");
        }

        private static object ApplyOperation(object oldValue, object newValue, string? operation)
        {
            if (operation == null || operation == "=")
                return newValue;

            dynamic a = oldValue;
            dynamic b = newValue;

            return operation switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => a / b,
                _ => newValue
            };
        }
    }
}

