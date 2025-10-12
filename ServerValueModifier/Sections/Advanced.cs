using Greed.Models;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Config;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services;
using System.Collections;
using System.Reflection;

namespace ServerValueModifier.Sections
{
    internal class Advanced(ISptLogger<SVM> logger, ConfigServer configServer, DatabaseService databaseService, MainClass.MainConfig svmconfig)
    {
        public void ItemChangerSection()
        {
            Dictionary<MongoId, TemplateItem> items = databaseService.GetItems();
            if (svmconfig.Custom.IDChanger)
            {
                logger.Info("[SVM] Custom Properties is loading");
                if (svmconfig.Custom.IDDefault.Length > 0 && svmconfig.Custom.IDDefault != "")
                {
                    string[] defaultList = svmconfig.Custom.IDDefault.Split("\r\n");
                    foreach (string line in defaultList)
                    {
                        if (!line.StartsWith("#") && !line.StartsWith("//"))
                        {
                            string[] variables = line.Split(":");
                            logger.Info("Default: " + line);
                            IDChanger(variables,items);
                        }
                    }
                }
                if (svmconfig.Custom.IDParent.Length > 0 && svmconfig.Custom.IDParent != "")
                    {
                    string[] parentlist = svmconfig.Custom.IDParent.Split("\r\n");
                    foreach (string line in parentlist)
                    {
                        List<string> idarray = [];
                        if (!line.StartsWith("#") && !line.StartsWith("//") && line.Length > 1)
                        {
                            var variables = line.Split(":");
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
                if (!line.StartsWith("#") && !line.StartsWith("//") && svmconfig.Custom.AddTraderAssort != "")
                {
                    string[] variables = line.Split(":");
                    MongoId uid = new(); 
                    variables[0] = variables[0]  switch
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
                        "USD" => "5696686a4bdc2da3298b456a",
                        "RUB" => "5449016a4bdc2d6f028b456f",
                        "EUR" => "569668774bdc2da2298b4568",
                        "GP" => "5d235b4d86f7742e017bc88a",
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
                string[] IDlist = svmconfig.Custom.FleaMultID.Split("\r\n");
                var fleaconfig = configServer.GetConfig<RagfairConfig>();
                fleaconfig.Dynamic.GenerateBaseFleaPrices.ItemTplMultiplierOverride = [];
                foreach (string line in IDlist)
                {
                    if (!line.StartsWith("#") && !line.StartsWith("//"))
                    {
                        string[] variables = line.Split(":");
                        fleaconfig.Dynamic.GenerateBaseFleaPrices.ItemTplMultiplierOverride.Add(variables[0], Convert.ToDouble(variables[1]));
                    }
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
                //Dictionary<MongoId, TemplateItem> items = databaseService.GetItems();
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
        //What is does is cycle through layers, checking whether it's operand, property or dataset, then adds stringValue on last member of Path and applies it to root which is our items
        public static void SetNestedValue(object obj, string[] path, string stringValue)
        {
            object current = obj;
            Type currentType = obj.GetType();

            // Last element may be an operand
            string lastToken = path.Last();
            string op = null;
            if (new[] { "+", "-", "*", "/", "=" }.Contains(lastToken))
            {
                op = lastToken;
                path = path.Take(path.Length - 1).ToArray();
            }

            // Walk down the path until the last segment
            for (int i = 0; i < path.Length - 1; i++)
            {
                string token = path[i];

                if (int.TryParse(token, out int arrayIndex))
                {
                    if (current is Array arr)
                    {
                        current = arr.GetValue(arrayIndex);
                        currentType = current.GetType();
                    }
                    else if (current is IList list)
                    {
                        current = list[arrayIndex];
                        currentType = current.GetType();
                    }
                }
                else
                {
                    var member = currentType.GetMember(token,
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();

                    current = member switch
                    {
                        PropertyInfo p => p.GetValue(current),
                        FieldInfo f => f.GetValue(current),
                        _ => throw new Exception($"Member {token} not found on {currentType.Name}")
                    };

                    currentType = current.GetType();
                }
            }

            // Get final target
            string finalToken = path.Last();
            object container = current;
            Type containerType = container.GetType();

            MemberInfo targetMember = containerType.GetMember(finalToken,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault();

            if (targetMember == null)
                throw new Exception($"Member {finalToken} not found on {containerType.Name}");

            Type targetType = targetMember is PropertyInfo pi ? pi.PropertyType : ((FieldInfo)targetMember).FieldType;
            Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            object currentValue = targetMember switch
            {
                PropertyInfo p => p.GetValue(container),
                FieldInfo f => f.GetValue(container),
                _ => null
            };

            // If target is collection
            if (underlyingType.IsGenericType && underlyingType.GetGenericTypeDefinition() == typeof(HashSet<>))
            {
                var elemType = underlyingType.GetGenericArguments()[0];
                var hashset = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elemType));

                foreach (var s in stringValue.Trim('[', ']').Split(',').Select(v => v.Trim()))
                {
                    object element;
                    if (elemType == typeof(string))
                    {
                        element = s;
                    }
                    else if (elemType == typeof(MongoId))
                    {
                        element = (MongoId)s;
                    }
                    else if (elemType.IsEnum)
                    {
                        element = Enum.Parse(elemType, s);
                    }
                    else
                    {
                        // Try to find a ctor(string)
                        var ctor = elemType.GetConstructor(new[] { typeof(string) });
                        if (ctor != null)
                        {
                            element = ctor.Invoke(new object[] { s });
                        }
                        else
                        {
                            throw new InvalidCastException(
                                $"Cannot convert string '{s}' to {elemType}. Provide a constructor or custom parser."
                            );
                        }
                    }
                    hashset.Add(element);
                }

                var newSet = Activator.CreateInstance(underlyingType, hashset);
                SetMemberValue(container, targetMember, newSet);
                return;
            }

            // Handle expressions only on last step
            object newValue;
            if (op != null)
            {
                double currentNum = Convert.ToDouble(currentValue);
                double delta = Convert.ToDouble(stringValue);

                newValue = op switch
                {
                    "+" => currentNum + delta,
                    "-" => currentNum - delta,
                    "*" => currentNum * delta,
                    "/" => currentNum / delta,
                    "=" => delta,
                    _ => throw new Exception("Unsupported operator")
                };
            }
            else
            {
                newValue = stringValue;
            }

            // Convert final value to correct type
            newValue = Convert.ChangeType(newValue, underlyingType);
            SetMemberValue(container, targetMember, newValue);
        }

        private static void SetMemberValue(object obj, MemberInfo member, object value)
        {
            switch (member)
            {
                case PropertyInfo p:
                    p.SetValue(obj, value);
                    break;
                case FieldInfo f:
                    f.SetValue(obj, value);
                    break;
                default:
                    throw new Exception("Unsupported member type");
            }
        }

    }
}

