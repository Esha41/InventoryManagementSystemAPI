using System;

namespace Ettad.CrossCutting.Comman.FileUpload
{
    /// <summary>
    /// Compile-time identifiers for product-defined attachment slots stored as
    /// <c>AttachmentRequirement</c> rows with <c>ParentType = System</c>.
    /// Each value maps to a stable string Code persisted on the row, resolved at
    /// runtime by <c>ISystemAttachmentSlotResolver</c>.
    /// </summary>
    public enum SystemAttachmentSlotCode
    {
        WeaponAssociation = 1,
        OrderReceiverSignature = 2
    }

    /// <summary>
    /// Maps <see cref="SystemAttachmentSlotCode"/> values to the string Code stored
    /// on the <c>AttachmentRequirement</c> row. These strings are part of the data
    /// contract and must not change once seeded.
    /// </summary>
    public static class SystemAttachmentSlotCodeExtensions
    {
        public const string WeaponAssociationCode = "WEAPON_ASSOCIATION";
        public const string OrderReceiverSignatureCode = "ORDER_RECEIVER_SIGNATURE";

        public static string ToDbCode(this SystemAttachmentSlotCode code) => code switch
        {
            SystemAttachmentSlotCode.WeaponAssociation => WeaponAssociationCode,
            SystemAttachmentSlotCode.OrderReceiverSignature => OrderReceiverSignatureCode,
            _ => throw new ArgumentOutOfRangeException(nameof(code), code, "Unknown system attachment slot code.")
        };
    }
}
