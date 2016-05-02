using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DocScanner.LibCommon
{
    public static class EventCleaner
    {
        // Methods
        public static void ClearEvents(this object objectHasEvents)
        {
            if (objectHasEvents != null)
            {
                EventInfo[] events = objectHasEvents.GetType().GetEvents(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if ((events != null) && (events.Length >= 1))
                {
                    for (int i = 0; i < events.Length; i++)
                    {
                        try
                        {
                            EventInfo info = events[i];
                            FieldInfo field = info.DeclaringType.GetField(info.Name, BindingFlags.NonPublic | BindingFlags.Instance);
                            if (field != null)
                            {
                                field.SetValue(objectHasEvents, null);
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }

}